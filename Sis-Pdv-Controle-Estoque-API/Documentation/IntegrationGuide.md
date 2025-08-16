# PDV Control System API - Integration Guide

This comprehensive guide will help you integrate with the PDV Control System API, covering everything from initial setup to advanced integration patterns.

## Table of Contents

- [Getting Started](#getting-started)
- [API Architecture](#api-architecture)
- [Authentication & Security](#authentication--security)
- [Core Integration Patterns](#core-integration-patterns)
- [Business Workflows](#business-workflows)
- [Real-time Integration](#real-time-integration)
- [Error Handling & Resilience](#error-handling--resilience)
- [Performance Optimization](#performance-optimization)
- [Testing Strategies](#testing-strategies)
- [Deployment Considerations](#deployment-considerations)
- [Monitoring & Observability](#monitoring--observability)

## Getting Started

### Prerequisites

Before integrating with the PDV Control System API, ensure you have:

1. **API Access Credentials**
   - Base URL for the API environment
   - Valid user account with appropriate permissions
   - Understanding of your required permission levels

2. **Development Environment**
   - HTTP client library for your programming language
   - JSON parsing capabilities
   - Secure storage for tokens and credentials

3. **Network Requirements**
   - HTTPS support (TLS 1.2 or higher)
   - Ability to make outbound HTTP requests
   - Proper firewall configuration if needed

### Quick Start Checklist

- [ ] Obtain API credentials and base URL
- [ ] Set up development environment
- [ ] Test basic connectivity with health check endpoint
- [ ] Implement authentication flow
- [ ] Test a simple API call (e.g., get current user info)
- [ ] Implement error handling
- [ ] Set up logging and monitoring

### Base URL Structure

```
Production:  https://api.pdvsystem.com
Staging:     https://staging-api.pdvsystem.com
Development: https://dev-api.pdvsystem.com
```

All API endpoints follow the pattern: `{baseUrl}/api/v{version}/{resource}`

## API Architecture

### RESTful Design Principles

The PDV Control System API follows REST architectural principles:

- **Resource-based URLs**: Each endpoint represents a specific resource
- **HTTP Methods**: Standard HTTP verbs (GET, POST, PUT, DELETE, PATCH)
- **Stateless**: Each request contains all necessary information
- **JSON Format**: All requests and responses use JSON
- **HTTP Status Codes**: Standard codes indicate request outcomes

### API Versioning

The API uses URL-based versioning:

```
/api/v1/products    # Version 1.0
/api/v2/products    # Version 2.0 (future)
```

**Version Support Policy:**
- Current version: v1.0
- Backward compatibility maintained for at least 12 months
- Deprecation notices provided 6 months before removal
- New features added to latest version first

### Resource Organization

```
Authentication:     /api/v1/auth/*
User Management:    /api/v1/users/*
Products:          /api/v1/produto/*
Customers:         /api/v1/cliente/*
Suppliers:         /api/v1/fornecedor/*
Orders:            /api/v1/pedido/*
Inventory:         /api/v1/inventory/*
Reports:           /api/v1/reports/*
System:            /api/v1/system/*
```

## Authentication & Security

### JWT Token-Based Authentication

The API uses JWT (JSON Web Tokens) for authentication:

1. **Access Token**: Short-lived (60 minutes) for API access
2. **Refresh Token**: Long-lived (7 days) for obtaining new access tokens

### Authentication Flow

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant AuthService
    
    Client->>API: POST /auth/login (credentials)
    API->>AuthService: Validate credentials
    AuthService-->>API: User validated
    API-->>Client: Access + Refresh tokens
    
    Client->>API: API request + Access token
    API-->>Client: Response
    
    Note over Client,API: Access token expires
    
    Client->>API: POST /auth/refresh (refresh token)
    API-->>Client: New access + refresh tokens
```

### Security Best Practices

1. **Token Storage**
   ```javascript
   // ✅ Good: Secure storage
   localStorage.setItem('pdv_access_token', token); // For web apps
   // Use secure keychain/keystore for mobile apps
   
   // ❌ Bad: Insecure storage
   // Don't store in cookies without proper security flags
   // Don't log tokens or include in URLs
   ```

2. **Token Transmission**
   ```http
   Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```

3. **HTTPS Only**
   - Always use HTTPS in production
   - Validate SSL certificates
   - Use certificate pinning for mobile apps

4. **Token Lifecycle Management**
   ```javascript
   // Automatic token refresh
   class TokenManager {
     async getValidToken() {
       if (this.isTokenExpiringSoon()) {
         await this.refreshToken();
       }
       return this.accessToken;
     }
   }
   ```

### Permission System

The API uses a role-based permission system:

```json
{
  "user": {
    "roles": ["Manager", "Cashier"],
    "permissions": [
      "product.view",
      "product.create",
      "inventory.view",
      "sales.create"
    ]
  }
}
```

**Common Permissions:**
- `product.*` - Product management
- `inventory.*` - Inventory operations
- `sales.*` - Sales operations
- `user.manage` - User management
- `reports.view` - Report access
- `backup.*` - Backup operations

## Core Integration Patterns

### 1. CRUD Operations Pattern

```javascript
class ResourceManager {
  constructor(apiClient, resourcePath) {
    this.api = apiClient;
    this.path = resourcePath;
  }

  async list(filters = {}) {
    const query = new URLSearchParams(filters).toString();
    return await this.api.get(`${this.path}?${query}`);
  }

  async get(id) {
    return await this.api.get(`${this.path}/${id}`);
  }

  async create(data) {
    return await this.api.post(this.path, data);
  }

  async update(id, data) {
    return await this.api.put(`${this.path}/${id}`, data);
  }

  async delete(id) {
    return await this.api.delete(`${this.path}/${id}`);
  }
}

// Usage
const productManager = new ResourceManager(apiClient, '/api/v1/produto');
const products = await productManager.list({ page: 1, pageSize: 20 });
```

### 2. Pagination Pattern

```javascript
class PaginatedResource {
  constructor(apiClient, endpoint) {
    this.api = apiClient;
    this.endpoint = endpoint;
  }

  async *getAllPages(pageSize = 50) {
    let page = 1;
    let hasMore = true;

    while (hasMore) {
      const response = await this.api.get(
        `${this.endpoint}?page=${page}&pageSize=${pageSize}`
      );
      
      yield response.items;
      
      hasMore = page < response.totalPages;
      page++;
    }
  }

  async getAll() {
    const allItems = [];
    for await (const pageItems of this.getAllPages()) {
      allItems.push(...pageItems);
    }
    return allItems;
  }
}

// Usage
const products = new PaginatedResource(apiClient, '/api/v1/produto/paginated');
for await (const pageItems of products.getAllPages(100)) {
  console.log(`Processing ${pageItems.length} products`);
  // Process each page
}
```

### 3. Batch Operations Pattern

```javascript
class BatchProcessor {
  constructor(apiClient, batchSize = 10) {
    this.api = apiClient;
    this.batchSize = batchSize;
  }

  async processBatch(items, operation) {
    const results = [];
    
    for (let i = 0; i < items.length; i += this.batchSize) {
      const batch = items.slice(i, i + this.batchSize);
      const batchPromises = batch.map(item => operation(item));
      
      try {
        const batchResults = await Promise.allSettled(batchPromises);
        results.push(...batchResults);
      } catch (error) {
        console.error(`Batch ${i / this.batchSize + 1} failed:`, error);
      }
      
      // Rate limiting: wait between batches
      if (i + this.batchSize < items.length) {
        await this.delay(100);
      }
    }
    
    return results;
  }

  delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}

// Usage
const processor = new BatchProcessor(apiClient);
const results = await processor.processBatch(
  productsToUpdate,
  async (product) => await apiClient.put(`/api/v1/produto/${product.id}`, product)
);
```

## Business Workflows

### 1. Complete Sales Workflow

```javascript
class SalesWorkflow {
  constructor(apiClient) {
    this.api = apiClient;
  }

  async processSale(saleData) {
    try {
      // 1. Validate stock availability
      await this.validateStock(saleData.items);
      
      // 2. Create the sale
      const sale = await this.api.post('/api/v1/pedido', saleData);
      
      // 3. Process payment
      const payment = await this.processPayment(sale.id, saleData.payment);
      
      // 4. Update inventory
      await this.updateInventory(saleData.items);
      
      // 5. Generate receipt
      const receipt = await this.generateReceipt(sale.id);
      
      return {
        sale,
        payment,
        receipt,
        success: true
      };
      
    } catch (error) {
      // Rollback if needed
      await this.rollbackSale(saleData);
      throw error;
    }
  }

  async validateStock(items) {
    for (const item of items) {
      const stock = await this.api.get(`/api/v1/inventory/stock/${item.productId}`);
      if (stock.quantity < item.quantity) {
        throw new Error(`Insufficient stock for product ${item.productId}`);
      }
    }
  }

  async processPayment(saleId, paymentData) {
    return await this.api.post(`/api/v1/sales/${saleId}/payment`, paymentData);
  }

  async updateInventory(items) {
    const updates = items.map(item => ({
      productId: item.productId,
      quantity: -item.quantity, // Negative for sale
      reason: 'Sale transaction'
    }));

    return await this.api.post('/api/v1/inventory/batch-adjust', { updates });
  }

  async generateReceipt(saleId) {
    return await this.api.get(`/api/v1/sales/${saleId}/receipt`);
  }
}
```

### 2. Inventory Management Workflow

```javascript
class InventoryManager {
  constructor(apiClient) {
    this.api = apiClient;
  }

  async performStockTake(locationId) {
    // 1. Get current stock levels
    const currentStock = await this.api.get(`/api/v1/inventory/stock?location=${locationId}`);
    
    // 2. Perform physical count (this would be manual input)
    const physicalCount = await this.getPhysicalCount(locationId);
    
    // 3. Calculate discrepancies
    const discrepancies = this.calculateDiscrepancies(currentStock, physicalCount);
    
    // 4. Create adjustment entries
    const adjustments = discrepancies.map(disc => ({
      productId: disc.productId,
      quantity: disc.difference,
      reason: `Stock take adjustment - ${disc.reason}`,
      type: 'STOCK_TAKE'
    }));
    
    // 5. Apply adjustments
    const results = await this.api.post('/api/v1/inventory/batch-adjust', { adjustments });
    
    // 6. Generate stock take report
    const report = await this.generateStockTakeReport(locationId, discrepancies);
    
    return { adjustments: results, report };
  }

  async monitorLowStock() {
    const alerts = await this.api.get('/api/v1/inventory/alerts');
    
    for (const alert of alerts.lowStock) {
      await this.handleLowStockAlert(alert);
    }
    
    return alerts;
  }

  async handleLowStockAlert(alert) {
    // Auto-reorder logic
    if (alert.autoReorder) {
      const orderQuantity = alert.reorderPoint * 2; // Example logic
      await this.createPurchaseOrder(alert.productId, orderQuantity);
    }
    
    // Notify relevant personnel
    await this.sendLowStockNotification(alert);
  }
}
```

### 3. Reporting Workflow

```javascript
class ReportingService {
  constructor(apiClient) {
    this.api = apiClient;
  }

  async generateDailyReport(date) {
    const [sales, inventory, financial] = await Promise.all([
      this.getSalesData(date),
      this.getInventoryData(date),
      this.getFinancialData(date)
    ]);

    const report = {
      date,
      sales: {
        totalSales: sales.total,
        transactionCount: sales.count,
        averageTicket: sales.total / sales.count,
        topProducts: sales.topProducts
      },
      inventory: {
        lowStockItems: inventory.lowStock.length,
        stockMovements: inventory.movements.length,
        stockValue: inventory.totalValue
      },
      financial: {
        revenue: financial.revenue,
        profit: financial.profit,
        expenses: financial.expenses
      }
    };

    // Generate PDF report
    const pdfReport = await this.api.post('/api/v1/reports/daily', {
      data: report,
      format: 'pdf'
    });

    return { data: report, pdf: pdfReport };
  }

  async scheduleRecurringReports() {
    const schedules = [
      { type: 'daily', time: '08:00', recipients: ['manager@company.com'] },
      { type: 'weekly', day: 'monday', time: '09:00', recipients: ['owner@company.com'] },
      { type: 'monthly', day: 1, time: '10:00', recipients: ['accounting@company.com'] }
    ];

    for (const schedule of schedules) {
      await this.api.post('/api/v1/reports/schedule', schedule);
    }
  }
}
```

## Real-time Integration

### WebSocket Integration (Future Enhancement)

```javascript
class RealTimeUpdates {
  constructor(apiClient) {
    this.api = apiClient;
    this.ws = null;
    this.eventHandlers = new Map();
  }

  async connect() {
    const token = await this.api.getAccessToken();
    this.ws = new WebSocket(`wss://api.pdvsystem.com/ws?token=${token}`);
    
    this.ws.onmessage = (event) => {
      const data = JSON.parse(event.data);
      this.handleEvent(data.type, data.payload);
    };
    
    this.ws.onclose = () => {
      // Implement reconnection logic
      setTimeout(() => this.connect(), 5000);
    };
  }

  on(eventType, handler) {
    if (!this.eventHandlers.has(eventType)) {
      this.eventHandlers.set(eventType, []);
    }
    this.eventHandlers.get(eventType).push(handler);
  }

  handleEvent(type, payload) {
    const handlers = this.eventHandlers.get(type) || [];
    handlers.forEach(handler => handler(payload));
  }
}

// Usage
const realTime = new RealTimeUpdates(apiClient);
await realTime.connect();

realTime.on('inventory.low_stock', (data) => {
  console.log('Low stock alert:', data);
  // Update UI or trigger notifications
});

realTime.on('sales.new_transaction', (data) => {
  console.log('New sale:', data);
  // Update dashboard in real-time
});
```

### Polling-based Updates

```javascript
class PollingService {
  constructor(apiClient) {
    this.api = apiClient;
    this.intervals = new Map();
  }

  startPolling(endpoint, callback, intervalMs = 30000) {
    const intervalId = setInterval(async () => {
      try {
        const data = await this.api.get(endpoint);
        callback(data);
      } catch (error) {
        console.error('Polling error:', error);
      }
    }, intervalMs);

    this.intervals.set(endpoint, intervalId);
    return intervalId;
  }

  stopPolling(endpoint) {
    const intervalId = this.intervals.get(endpoint);
    if (intervalId) {
      clearInterval(intervalId);
      this.intervals.delete(endpoint);
    }
  }

  stopAllPolling() {
    this.intervals.forEach(intervalId => clearInterval(intervalId));
    this.intervals.clear();
  }
}

// Usage
const polling = new PollingService(apiClient);

// Poll for low stock alerts every 5 minutes
polling.startPolling('/api/v1/inventory/alerts', (alerts) => {
  if (alerts.lowStock.length > 0) {
    showLowStockNotification(alerts.lowStock);
  }
}, 300000);
```

## Error Handling & Resilience

### Comprehensive Error Handling

```javascript
class ApiError extends Error {
  constructor(message, status, code, details) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.code = code;
    this.details = details;
  }
}

class ResilientApiClient {
  constructor(baseUrl, options = {}) {
    this.baseUrl = baseUrl;
    this.maxRetries = options.maxRetries || 3;
    this.retryDelay = options.retryDelay || 1000;
    this.timeout = options.timeout || 30000;
  }

  async request(method, endpoint, data = null, options = {}) {
    const retryableStatuses = [408, 429, 500, 502, 503, 504];
    let lastError;

    for (let attempt = 0; attempt <= this.maxRetries; attempt++) {
      try {
        const response = await this.makeRequest(method, endpoint, data, options);
        return response;
      } catch (error) {
        lastError = error;

        // Don't retry on client errors (4xx except 408, 429)
        if (error.status >= 400 && error.status < 500 && 
            !retryableStatuses.includes(error.status)) {
          throw error;
        }

        // Don't retry on last attempt
        if (attempt === this.maxRetries) {
          break;
        }

        // Calculate delay with exponential backoff
        const delay = this.retryDelay * Math.pow(2, attempt);
        await this.sleep(delay);
      }
    }

    throw lastError;
  }

  async makeRequest(method, endpoint, data, options) {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), this.timeout);

    try {
      const response = await fetch(`${this.baseUrl}${endpoint}`, {
        method,
        headers: {
          'Content-Type': 'application/json',
          ...options.headers
        },
        body: data ? JSON.stringify(data) : null,
        signal: controller.signal
      });

      clearTimeout(timeoutId);

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new ApiError(
          errorData.message || 'Request failed',
          response.status,
          errorData.code,
          errorData.errors
        );
      }

      return await response.json();
    } catch (error) {
      clearTimeout(timeoutId);
      
      if (error.name === 'AbortError') {
        throw new ApiError('Request timeout', 408, 'TIMEOUT');
      }
      
      throw error;
    }
  }

  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
```

### Circuit Breaker Pattern

```javascript
class CircuitBreaker {
  constructor(threshold = 5, timeout = 60000) {
    this.threshold = threshold;
    this.timeout = timeout;
    this.failureCount = 0;
    this.lastFailureTime = null;
    this.state = 'CLOSED'; // CLOSED, OPEN, HALF_OPEN
  }

  async execute(operation) {
    if (this.state === 'OPEN') {
      if (Date.now() - this.lastFailureTime > this.timeout) {
        this.state = 'HALF_OPEN';
      } else {
        throw new Error('Circuit breaker is OPEN');
      }
    }

    try {
      const result = await operation();
      this.onSuccess();
      return result;
    } catch (error) {
      this.onFailure();
      throw error;
    }
  }

  onSuccess() {
    this.failureCount = 0;
    this.state = 'CLOSED';
  }

  onFailure() {
    this.failureCount++;
    this.lastFailureTime = Date.now();
    
    if (this.failureCount >= this.threshold) {
      this.state = 'OPEN';
    }
  }
}

// Usage
const circuitBreaker = new CircuitBreaker(3, 30000);

async function callApiWithCircuitBreaker(endpoint) {
  return await circuitBreaker.execute(async () => {
    return await apiClient.get(endpoint);
  });
}
```

## Performance Optimization

### Caching Strategies

```javascript
class CacheManager {
  constructor(defaultTtl = 300000) { // 5 minutes default
    this.cache = new Map();
    this.defaultTtl = defaultTtl;
  }

  set(key, value, ttl = this.defaultTtl) {
    const expiresAt = Date.now() + ttl;
    this.cache.set(key, { value, expiresAt });
  }

  get(key) {
    const item = this.cache.get(key);
    if (!item) return null;

    if (Date.now() > item.expiresAt) {
      this.cache.delete(key);
      return null;
    }

    return item.value;
  }

  clear() {
    this.cache.clear();
  }

  // Cache with automatic refresh
  async getOrFetch(key, fetchFn, ttl) {
    let value = this.get(key);
    if (value === null) {
      value = await fetchFn();
      this.set(key, value, ttl);
    }
    return value;
  }
}

class CachedApiClient {
  constructor(apiClient, cacheManager) {
    this.api = apiClient;
    this.cache = cacheManager;
  }

  async getCachedProducts(page = 1, pageSize = 20) {
    const cacheKey = `products:${page}:${pageSize}`;
    return await this.cache.getOrFetch(
      cacheKey,
      () => this.api.get(`/api/v1/produto/paginated?page=${page}&pageSize=${pageSize}`),
      300000 // 5 minutes
    );
  }

  async getCachedProduct(id) {
    const cacheKey = `product:${id}`;
    return await this.cache.getOrFetch(
      cacheKey,
      () => this.api.get(`/api/v1/produto/${id}`),
      600000 // 10 minutes
    );
  }

  // Invalidate cache when data changes
  async updateProduct(id, data) {
    const result = await this.api.put(`/api/v1/produto/${id}`, data);
    
    // Invalidate related cache entries
    this.cache.delete(`product:${id}`);
    // Could also invalidate product list cache
    
    return result;
  }
}
```

### Request Optimization

```javascript
class OptimizedApiClient {
  constructor(baseApiClient) {
    this.api = baseApiClient;
    this.requestQueue = [];
    this.processing = false;
  }

  // Batch similar requests
  async batchGet(endpoints) {
    const promises = endpoints.map(endpoint => this.api.get(endpoint));
    return await Promise.allSettled(promises);
  }

  // Request deduplication
  async deduplicatedRequest(endpoint) {
    const existingRequest = this.requestQueue.find(req => req.endpoint === endpoint);
    if (existingRequest) {
      return existingRequest.promise;
    }

    const promise = this.api.get(endpoint);
    this.requestQueue.push({ endpoint, promise });

    try {
      const result = await promise;
      return result;
    } finally {
      this.requestQueue = this.requestQueue.filter(req => req.endpoint !== endpoint);
    }
  }

  // Parallel processing with concurrency limit
  async processWithConcurrencyLimit(tasks, limit = 5) {
    const results = [];
    
    for (let i = 0; i < tasks.length; i += limit) {
      const batch = tasks.slice(i, i + limit);
      const batchResults = await Promise.allSettled(batch);
      results.push(...batchResults);
    }
    
    return results;
  }
}
```

## Testing Strategies

### Unit Testing

```javascript
// Mock API client for testing
class MockApiClient {
  constructor() {
    this.responses = new Map();
    this.callLog = [];
  }

  mockResponse(endpoint, response) {
    this.responses.set(endpoint, response);
  }

  async get(endpoint) {
    this.callLog.push({ method: 'GET', endpoint });
    
    const response = this.responses.get(endpoint);
    if (!response) {
      throw new Error(`No mock response for ${endpoint}`);
    }
    
    return response;
  }

  getCallLog() {
    return this.callLog;
  }

  clearCallLog() {
    this.callLog = [];
  }
}

// Test example
describe('ProductManager', () => {
  let mockApi;
  let productManager;

  beforeEach(() => {
    mockApi = new MockApiClient();
    productManager = new ProductManager(mockApi);
  });

  test('should fetch products with pagination', async () => {
    const mockProducts = {
      items: [{ id: '1', nome: 'Product 1' }],
      totalPages: 1,
      currentPage: 1
    };
    
    mockApi.mockResponse('/api/v1/produto/paginated?page=1&pageSize=20', mockProducts);
    
    const result = await productManager.getProducts(1, 20);
    
    expect(result).toEqual(mockProducts);
    expect(mockApi.getCallLog()).toHaveLength(1);
  });
});
```

### Integration Testing

```javascript
class IntegrationTestSuite {
  constructor(apiBaseUrl) {
    this.api = new PdvApiClient(apiBaseUrl);
    this.testData = new Map();
  }

  async setup() {
    // Login with test credentials
    await this.api.login('test@example.com', 'testpassword');
    
    // Create test data
    const testProduct = await this.api.createProduct({
      nome: 'Test Product',
      descricao: 'Test Description',
      preco: 10.00,
      codigoBarras: '1234567890123'
    });
    
    this.testData.set('product', testProduct);
  }

  async cleanup() {
    // Clean up test data
    for (const [key, data] of this.testData) {
      try {
        if (key === 'product') {
          await this.api.deleteProduct(data.id);
        }
      } catch (error) {
        console.warn(`Failed to cleanup ${key}:`, error);
      }
    }
    
    await this.api.logout();
  }

  async testCompleteWorkflow() {
    const product = this.testData.get('product');
    
    // Test inventory adjustment
    await this.api.adjustStock(product.id, 100, 'Initial stock');
    
    // Test stock query
    const movements = await this.api.getStockMovements(product.id);
    expect(movements.length).toBeGreaterThan(0);
    
    // Test sales workflow
    const sale = await this.api.createSale({
      items: [{ productId: product.id, quantity: 1, price: 10.00 }],
      customerId: null,
      paymentMethod: 'cash'
    });
    
    expect(sale.total).toBe(10.00);
  }
}
```

## Deployment Considerations

### Environment Configuration

```javascript
class EnvironmentConfig {
  constructor() {
    this.config = {
      development: {
        apiUrl: 'https://dev-api.pdvsystem.com',
        timeout: 30000,
        retries: 3,
        logLevel: 'debug'
      },
      staging: {
        apiUrl: 'https://staging-api.pdvsystem.com',
        timeout: 15000,
        retries: 2,
        logLevel: 'info'
      },
      production: {
        apiUrl: 'https://api.pdvsystem.com',
        timeout: 10000,
        retries: 1,
        logLevel: 'error'
      }
    };
  }

  get(environment = 'production') {
    return this.config[environment] || this.config.production;
  }
}

// Usage
const config = new EnvironmentConfig();
const apiClient = new PdvApiClient(config.get(process.env.NODE_ENV));
```

### Health Checks

```javascript
class HealthChecker {
  constructor(apiClient) {
    this.api = apiClient;
  }

  async checkHealth() {
    const checks = {
      api: false,
      auth: false,
      database: false,
      timestamp: new Date().toISOString()
    };

    try {
      // Basic API connectivity
      await this.api.get('/api/v1/health');
      checks.api = true;

      // Authentication system
      const authCheck = await this.api.get('/api/v1/health/auth');
      checks.auth = authCheck.status === 'healthy';

      // Database connectivity
      const dbCheck = await this.api.get('/api/v1/health/database');
      checks.database = dbCheck.status === 'healthy';

    } catch (error) {
      console.error('Health check failed:', error);
    }

    return checks;
  }

  async waitForHealthy(maxAttempts = 30, delayMs = 2000) {
    for (let attempt = 1; attempt <= maxAttempts; attempt++) {
      const health = await this.checkHealth();
      
      if (health.api && health.auth && health.database) {
        return health;
      }

      console.log(`Health check attempt ${attempt}/${maxAttempts} failed, retrying...`);
      await new Promise(resolve => setTimeout(resolve, delayMs));
    }

    throw new Error('Service did not become healthy within timeout period');
  }
}
```

## Monitoring & Observability

### Logging and Metrics

```javascript
class ApiMetrics {
  constructor() {
    this.metrics = {
      requests: 0,
      errors: 0,
      responseTime: [],
      errorsByType: new Map()
    };
  }

  recordRequest(duration, success, error = null) {
    this.metrics.requests++;
    this.metrics.responseTime.push(duration);

    if (!success) {
      this.metrics.errors++;
      if (error) {
        const errorType = error.constructor.name;
        this.metrics.errorsByType.set(
          errorType,
          (this.metrics.errorsByType.get(errorType) || 0) + 1
        );
      }
    }
  }

  getStats() {
    const responseTimes = this.metrics.responseTime;
    return {
      totalRequests: this.metrics.requests,
      totalErrors: this.metrics.errors,
      errorRate: this.metrics.errors / this.metrics.requests,
      avgResponseTime: responseTimes.reduce((a, b) => a + b, 0) / responseTimes.length,
      maxResponseTime: Math.max(...responseTimes),
      minResponseTime: Math.min(...responseTimes),
      errorsByType: Object.fromEntries(this.metrics.errorsByType)
    };
  }

  reset() {
    this.metrics = {
      requests: 0,
      errors: 0,
      responseTime: [],
      errorsByType: new Map()
    };
  }
}

class MonitoredApiClient {
  constructor(baseApiClient, metrics) {
    this.api = baseApiClient;
    this.metrics = metrics;
  }

  async request(method, endpoint, data) {
    const startTime = Date.now();
    let success = false;
    let error = null;

    try {
      const result = await this.api.request(method, endpoint, data);
      success = true;
      return result;
    } catch (err) {
      error = err;
      throw err;
    } finally {
      const duration = Date.now() - startTime;
      this.metrics.recordRequest(duration, success, error);
    }
  }
}

// Usage
const metrics = new ApiMetrics();
const monitoredApi = new MonitoredApiClient(apiClient, metrics);

// Periodic metrics reporting
setInterval(() => {
  const stats = metrics.getStats();
  console.log('API Metrics:', stats);
  
  // Send to monitoring service
  // sendToMonitoring(stats);
}, 60000); // Every minute
```

### Error Tracking

```javascript
class ErrorTracker {
  constructor(options = {}) {
    this.errors = [];
    this.maxErrors = options.maxErrors || 1000;
    this.onError = options.onError || console.error;
  }

  track(error, context = {}) {
    const errorInfo = {
      timestamp: new Date().toISOString(),
      message: error.message,
      stack: error.stack,
      type: error.constructor.name,
      context,
      id: this.generateId()
    };

    this.errors.unshift(errorInfo);
    
    // Keep only recent errors
    if (this.errors.length > this.maxErrors) {
      this.errors = this.errors.slice(0, this.maxErrors);
    }

    this.onError(errorInfo);
    return errorInfo.id;
  }

  getErrors(limit = 50) {
    return this.errors.slice(0, limit);
  }

  getErrorById(id) {
    return this.errors.find(error => error.id === id);
  }

  generateId() {
    return Math.random().toString(36).substr(2, 9);
  }
}

// Global error tracking
const errorTracker = new ErrorTracker({
  onError: (error) => {
    // Send to external error tracking service
    // Sentry, Rollbar, etc.
    console.error('Tracked error:', error);
  }
});

// Wrap API client with error tracking
class TrackedApiClient {
  constructor(apiClient, errorTracker) {
    this.api = apiClient;
    this.errorTracker = errorTracker;
  }

  async request(method, endpoint, data) {
    try {
      return await this.api.request(method, endpoint, data);
    } catch (error) {
      const errorId = this.errorTracker.track(error, {
        method,
        endpoint,
        data: JSON.stringify(data).substring(0, 1000) // Truncate large payloads
      });
      
      // Add error ID to the error for reference
      error.trackingId = errorId;
      throw error;
    }
  }
}
```

This comprehensive integration guide provides everything needed to successfully integrate with the PDV Control System API, from basic setup to advanced patterns and production considerations.