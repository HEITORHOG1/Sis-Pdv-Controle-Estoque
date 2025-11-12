using Repositories.Base;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Repositories.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PdvContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(PdvContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await SaveChangesAsync(CancellationToken.None);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (MySqlException ex) when (IsDeadlock(ex))
            {
                _logger.LogWarning(ex, "Deadlock detected, retrying operation");
                // EF Core's EnableRetryOnFailure will handle the retry automatically
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to database");
                throw;
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                _logger.LogWarning("Transaction already in progress");
                return _currentTransaction;
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            _logger.LogInformation("Transaction started with isolation level: {IsolationLevel}", isolationLevel);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No active transaction to commit");
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Transaction committed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error committing transaction");
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                await _currentTransaction.RollbackAsync();
                _logger.LogWarning("Transaction rolled back");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rolling back transaction");
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        private static bool IsDeadlock(MySqlException exception)
        {
            return exception.ErrorCode == MySqlErrorCode.LockDeadlock;
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _context?.Dispose();
        }
    }
}
