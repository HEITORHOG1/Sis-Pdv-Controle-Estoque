#!/usr/bin/env pwsh

# PDV System Test Runner Script
# This script runs all tests in the comprehensive test suite

param(
    [string]$TestCategory = "All",
    [switch]$Coverage = $false,
    [switch]$Verbose = $false
)

Write-Host "PDV System Test Suite Runner" -ForegroundColor Cyan
Write-Host "=================================" -ForegroundColor Cyan

# Set test project path
$TestProject = "Sis-Pdv-Controle-Estoque.Tests.csproj"

# Base dotnet test command
$TestCommand = "dotnet test $TestProject"

# Add verbosity if requested
if ($Verbose) {
    $TestCommand += " --verbosity normal"
}

# Add coverage if requested
if ($Coverage) {
    $TestCommand += " --collect:`"XPlat Code Coverage`""
}

# Run tests based on category
switch ($TestCategory.ToLower()) {
    "unit" {
        Write-Host "Running Unit Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter Category=Unit"
    }
    "integration" {
        Write-Host "Running Integration Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter Category=Integration"
    }
    "e2e" {
        Write-Host "Running End-to-End Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter Category=E2E"
    }
    "handlers" {
        Write-Host "Running Handler Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter FullyQualifiedName~Handlers"
    }
    "repositories" {
        Write-Host "Running Repository Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter FullyQualifiedName~Repositories"
    }
    "controllers" {
        Write-Host "Running Controller Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter FullyQualifiedName~Controllers"
    }
    "validators" {
        Write-Host "Running Validator Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter FullyQualifiedName~Validators"
    }
    "services" {
        Write-Host "Running Service Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter FullyQualifiedName~Services"
    }
    "workflows" {
        Write-Host "Running Workflow Tests..." -ForegroundColor Yellow
        $TestCommand += " --filter FullyQualifiedName~EndToEnd"
    }
    default {
        Write-Host "Running All Tests..." -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "Executing: $TestCommand" -ForegroundColor Gray
Write-Host ""

# Execute the test command
try {
    Invoke-Expression $TestCommand
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "All tests passed successfully!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "Some tests failed. Check the output above for details." -ForegroundColor Red
        exit $LASTEXITCODE
    }
} catch {
    Write-Host ""
    Write-Host "Error running tests: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Show coverage report location if coverage was collected
if ($Coverage) {
    Write-Host ""
    Write-Host "Code coverage report generated in TestResults folder" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "Test Categories Available:" -ForegroundColor Cyan
Write-Host "  unit         - Unit tests only" -ForegroundColor Gray
Write-Host "  integration  - Integration tests only" -ForegroundColor Gray
Write-Host "  e2e          - End-to-end tests only" -ForegroundColor Gray
Write-Host "  handlers     - Handler tests only" -ForegroundColor Gray
Write-Host "  repositories - Repository tests only" -ForegroundColor Gray
Write-Host "  controllers  - Controller tests only" -ForegroundColor Gray
Write-Host "  validators   - Validator tests only" -ForegroundColor Gray
Write-Host "  services     - Service tests only" -ForegroundColor Gray
Write-Host "  workflows    - Workflow tests only" -ForegroundColor Gray
Write-Host ""
Write-Host "Usage Examples:" -ForegroundColor Cyan
Write-Host "  .\run-tests.ps1                    # Run all tests" -ForegroundColor Gray
Write-Host "  .\run-tests.ps1 -TestCategory unit # Run unit tests only" -ForegroundColor Gray
Write-Host "  .\run-tests.ps1 -Coverage          # Run with coverage" -ForegroundColor Gray
Write-Host "  .\run-tests.ps1 -Verbose           # Run with verbose output" -ForegroundColor Gray