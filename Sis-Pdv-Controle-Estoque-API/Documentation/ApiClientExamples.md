# PDV Control System API - Client Examples

This document provides comprehensive examples for integrating with the PDV Control System API using various programming languages and tools.

## Table of Contents

- [Authentication](#authentication)
- [JavaScript/TypeScript Examples](#javascripttypescript-examples)
- [C# Examples](#c-examples)
- [Python Examples](#python-examples)
- [cURL Examples](#curl-examples)
- [Postman Collection](#postman-collection)
- [Error Handling](#error-handling)
- [Rate Limiting](#rate-limiting)

## Authentication

All API endpoints (except authentication endpoints) require a valid JWT token in the Authorization header:

```
Authorization: Bearer <your-access-token>
```

### Authentication Flow

1. **Login** - Get access and refresh tokens
2. **Use Access Token** - Include in Authorization header for API calls
3. **Refresh Token** - Get new access token when current expires
4. **Logout** - Revoke refresh token when done

## JavaScript/TypeScript Examples

### Basic API Client Class

```typescript
interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: string[];
}

interface AuthResult {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: {
    id: string;
    login: string;
    email: string;
    nome: string;
    roles: string[];
    permissions: string[];
  };
}

class PdvApiClient {
  private baseUrl: string;
  private accessToken: string | null = null;
  private refreshToken: string | null = null;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl.replace(/\/$/, ''); // Remove trailing slash
  }

  // Authentication
  async login(login: string, password: string): Promise<AuthResult> {
    const response = await fetch(`${this.baseUrl}/api/v1/auth/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ login, password }),
    });

    const result: ApiResponse<AuthResult> = await response.json();
    
    if (!result.success) {
      throw new Error(result.message);
    }

    this.accessToken = result.data!.accessToken;
    this.refreshToken = result.data!.refreshToken;
    
    return result.data!;
  }

  async refreshAccessToken(): Promise<AuthResult> {
    if (!this.refreshToken) {
      throw new Error('No refresh token available');
    }

    const response = await fetch(`${this.baseUrl}/api/v1/auth/refresh`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ refreshToken: this.refreshToken }),
    });

    const result: ApiResponse<AuthResult> = await response.json();
    
    if (!result.success) {
      throw new Error(result.message);
    }

    this.accessToken = result.data!.accessToken;
    this.refreshToken = result.data!.refreshToken;
    
    return result.data!;
  }

  async logout(): Promise<void> {
    if (!this.refreshToken) return;

    await fetch(`${this.baseUrl}/api/v1/auth/logout`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ refreshToken: this.refreshToken }),
    });

    this.accessToken = null;
    this.refreshToken = null;
  }

  // Generic API call with automatic token refresh
  private async apiCall<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    const url = `${this.baseUrl}${endpoint}`;
    const headers = {
      'Content-Type': 'application/json',
      ...options.headers,
    };

    if (this.accessToken) {
      headers['Authorization'] = `Bearer ${this.accessToken}`;
    }

    let response = await fetch(url, {
      ...options,
      headers,
    });

    // Handle token expiration
    if (response.status === 401 && this.refreshToken) {
      try {
        await this.refreshAccessToken();
        headers['Authorization'] = `Bearer ${this.accessToken}`;
        response = await fetch(url, {
          ...options,
          headers,
        });
      } catch (error) {
        throw new Error('Authentication failed. Please login again.');
      }
    }

    return await response.json();
  }

  // Product operations
  async getProducts(page: number = 1, pageSize: number = 20): Promise<any> {
    const result = await this.apiCall(
      `/api/v1/produto/paginated?page=${page}&pageSize=${pageSize}`
    );
    return result.data;
  }

  async createProduct(product: any): Promise<any> {
    const result = await this.apiCall('/api/v1/produto', {
      method: 'POST',
      body: JSON.stringify(product),
    });
    return result.data;
  }

  async updateProduct(id: string, product: any): Promise<any> {
    const result = await this.apiCall(`/api/v1/produto/${id}`, {
      method: 'PUT',
      body: JSON.stringify(product),
    });
    return result.data;
  }

  async deleteProduct(id: string): Promise<void> {
    await this.apiCall(`/api/v1/produto/${id}`, {
      method: 'DELETE',
    });
  }

  // Inventory operations
  async getStockMovements(productId?: string): Promise<any> {
    const query = productId ? `?productId=${productId}` : '';
    const result = await this.apiCall(`/api/v1/inventory/movements${query}`);
    return result.data;
  }

  async adjustStock(productId: string, quantity: number, reason: string): Promise<any> {
    const result = await this.apiCall('/api/v1/inventory/adjust', {
      method: 'POST',
      body: JSON.stringify({ productId, quantity, reason }),
    });
    return result.data;
  }

  // Reports
  async generateSalesReport(startDate: string, endDate: string, format: 'pdf' | 'excel' = 'pdf'): Promise<Blob> {
    const response = await fetch(
      `${this.baseUrl}/api/v1/reports/sales?startDate=${startDate}&endDate=${endDate}&format=${format}`,
      {
        headers: {
          'Authorization': `Bearer ${this.accessToken}`,
        },
      }
    );

    if (!response.ok) {
      throw new Error('Failed to generate report');
    }

    return await response.blob();
  }
}

// Usage example
async function example() {
  const client = new PdvApiClient('https://api.pdvsystem.com');

  try {
    // Login
    const authResult = await client.login('admin@pdvsystem.com', 'password123');
    console.log('Logged in as:', authResult.user.nome);

    // Get products
    const products = await client.getProducts(1, 10);
    console.log('Products:', products);

    // Create a new product
    const newProduct = await client.createProduct({
      nome: 'New Product',
      descricao: 'Product description',
      preco: 29.99,
      codigoBarras: '1234567890123',
    });
    console.log('Created product:', newProduct);

    // Generate sales report
    const reportBlob = await client.generateSalesReport('2024-01-01', '2024-01-31', 'pdf');
    
    // Download the report
    const url = URL.createObjectURL(reportBlob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'sales-report.pdf';
    a.click();

  } catch (error) {
    console.error('API Error:', error);
  } finally {
    await client.logout();
  }
}
```

### React Hook Example

```typescript
import { useState, useEffect, useCallback } from 'react';

interface UseApiResult<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
  refetch: () => void;
}

function useApi<T>(endpoint: string, dependencies: any[] = []): UseApiResult<T> {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      
      const client = new PdvApiClient(process.env.REACT_APP_API_URL!);
      const result = await client.apiCall<T>(endpoint);
      
      if (result.success) {
        setData(result.data!);
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unknown error');
    } finally {
      setLoading(false);
    }
  }, [endpoint, ...dependencies]);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return { data, loading, error, refetch: fetchData };
}

// Usage in React component
function ProductList() {
  const { data: products, loading, error, refetch } = useApi<any[]>('/api/v1/produto/paginated');

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <button onClick={refetch}>Refresh</button>
      <ul>
        {products?.map(product => (
          <li key={product.id}>{product.nome} - ${product.preco}</li>
        ))}
      </ul>
    </div>
  );
}
```

## C# Examples

### HttpClient-based API Client

```csharp
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class PdvApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private string? _accessToken;
    private string? _refreshToken;

    public PdvApiClient(string baseUrl)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<AuthResult> LoginAsync(string login, string password)
    {
        var request = new { login, password };
        var response = await PostAsync<AuthResult>("/api/v1/auth/login", request);
        
        _accessToken = response.AccessToken;
        _refreshToken = response.RefreshToken;
        
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _accessToken);
        
        return response;
    }

    public async Task<AuthResult> RefreshTokenAsync()
    {
        if (string.IsNullOrEmpty(_refreshToken))
            throw new InvalidOperationException("No refresh token available");

        var request = new { refreshToken = _refreshToken };
        var response = await PostAsync<AuthResult>("/api/v1/auth/refresh", request);
        
        _accessToken = response.AccessToken;
        _refreshToken = response.RefreshToken;
        
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _accessToken);
        
        return response;
    }

    public async Task LogoutAsync()
    {
        if (!string.IsNullOrEmpty(_refreshToken))
        {
            var request = new { refreshToken = _refreshToken };
            await PostAsync<bool>("/api/v1/auth/logout", request);
        }
        
        _accessToken = null;
        _refreshToken = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        await EnsureSuccessStatusCodeAsync(response);
        
        var json = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        if (!apiResponse.Success)
            throw new ApiException(apiResponse.Message);
        
        return apiResponse.Data;
    }

    private async Task<T> PostAsync<T>(string endpoint, object data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        await EnsureSuccessStatusCodeAsync(response);
        
        var responseJson = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseJson, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        if (!apiResponse.Success)
            throw new ApiException(apiResponse.Message);
        
        return apiResponse.Data;
    }

    private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(_refreshToken))
        {
            try
            {
                await RefreshTokenAsync();
                // Retry the original request would require more complex implementation
            }
            catch
            {
                throw new UnauthorizedAccessException("Authentication failed. Please login again.");
            }
        }
        
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

// Data models
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T Data { get; set; } = default!;
    public List<string>? Errors { get; set; }
}

public class AuthResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public UserInfo User { get; set; } = new();
}

public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}

public class ApiException : Exception
{
    public ApiException(string message) : base(message) { }
}

// Usage example
class Program
{
    static async Task Main(string[] args)
    {
        using var client = new PdvApiClient("https://api.pdvsystem.com");
        
        try
        {
            // Login
            var authResult = await client.LoginAsync("admin@pdvsystem.com", "password123");
            Console.WriteLine($"Logged in as: {authResult.User.Nome}");
            
            // Use API endpoints
            // var products = await client.GetAsync<List<Product>>("/api/v1/produto/paginated");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            await client.LogoutAsync();
        }
    }
}
```

## Python Examples

### Requests-based API Client

```python
import requests
import json
from typing import Optional, Dict, Any
from datetime import datetime, timedelta

class PdvApiClient:
    def __init__(self, base_url: str):
        self.base_url = base_url.rstrip('/')
        self.session = requests.Session()
        self.access_token: Optional[str] = None
        self.refresh_token: Optional[str] = None
        self.token_expires_at: Optional[datetime] = None
    
    def login(self, login: str, password: str) -> Dict[str, Any]:
        """Authenticate and obtain access tokens"""
        response = self.session.post(
            f"{self.base_url}/api/v1/auth/login",
            json={"login": login, "password": password}
        )
        response.raise_for_status()
        
        result = response.json()
        if not result.get('success'):
            raise Exception(result.get('message', 'Login failed'))
        
        data = result['data']
        self.access_token = data['accessToken']
        self.refresh_token = data['refreshToken']
        self.token_expires_at = datetime.now() + timedelta(seconds=data['expiresIn'])
        
        # Set authorization header for future requests
        self.session.headers.update({
            'Authorization': f"Bearer {self.access_token}"
        })
        
        return data
    
    def refresh_access_token(self) -> Dict[str, Any]:
        """Refresh the access token using refresh token"""
        if not self.refresh_token:
            raise Exception("No refresh token available")
        
        response = self.session.post(
            f"{self.base_url}/api/v1/auth/refresh",
            json={"refreshToken": self.refresh_token}
        )
        response.raise_for_status()
        
        result = response.json()
        if not result.get('success'):
            raise Exception(result.get('message', 'Token refresh failed'))
        
        data = result['data']
        self.access_token = data['accessToken']
        self.refresh_token = data['refreshToken']
        self.token_expires_at = datetime.now() + timedelta(seconds=data['expiresIn'])
        
        # Update authorization header
        self.session.headers.update({
            'Authorization': f"Bearer {self.access_token}"
        })
        
        return data
    
    def logout(self):
        """Logout and revoke refresh token"""
        if self.refresh_token:
            try:
                self.session.post(
                    f"{self.base_url}/api/v1/auth/logout",
                    json={"refreshToken": self.refresh_token}
                )
            except:
                pass  # Ignore errors during logout
        
        self.access_token = None
        self.refresh_token = None
        self.token_expires_at = None
        
        # Remove authorization header
        if 'Authorization' in self.session.headers:
            del self.session.headers['Authorization']
    
    def _ensure_valid_token(self):
        """Ensure we have a valid access token"""
        if not self.access_token:
            raise Exception("Not authenticated. Please login first.")
        
        # Check if token is about to expire (refresh 5 minutes before expiration)
        if self.token_expires_at and datetime.now() >= self.token_expires_at - timedelta(minutes=5):
            self.refresh_access_token()
    
    def _api_call(self, method: str, endpoint: str, **kwargs) -> Dict[str, Any]:
        """Make an authenticated API call"""
        self._ensure_valid_token()
        
        url = f"{self.base_url}{endpoint}"
        response = self.session.request(method, url, **kwargs)
        
        # Handle token expiration
        if response.status_code == 401 and self.refresh_token:
            try:
                self.refresh_access_token()
                response = self.session.request(method, url, **kwargs)
            except:
                raise Exception("Authentication failed. Please login again.")
        
        response.raise_for_status()
        
        result = response.json()
        if not result.get('success'):
            raise Exception(result.get('message', 'API call failed'))
        
        return result.get('data')
    
    # Product operations
    def get_products(self, page: int = 1, page_size: int = 20) -> Dict[str, Any]:
        """Get paginated list of products"""
        return self._api_call(
            'GET', 
            f"/api/v1/produto/paginated?page={page}&pageSize={page_size}"
        )
    
    def create_product(self, product_data: Dict[str, Any]) -> Dict[str, Any]:
        """Create a new product"""
        return self._api_call('POST', '/api/v1/produto', json=product_data)
    
    def update_product(self, product_id: str, product_data: Dict[str, Any]) -> Dict[str, Any]:
        """Update an existing product"""
        return self._api_call('PUT', f'/api/v1/produto/{product_id}', json=product_data)
    
    def delete_product(self, product_id: str):
        """Delete a product"""
        self._api_call('DELETE', f'/api/v1/produto/{product_id}')
    
    # Inventory operations
    def get_stock_movements(self, product_id: Optional[str] = None) -> Dict[str, Any]:
        """Get stock movements"""
        endpoint = '/api/v1/inventory/movements'
        if product_id:
            endpoint += f'?productId={product_id}'
        return self._api_call('GET', endpoint)
    
    def adjust_stock(self, product_id: str, quantity: float, reason: str) -> Dict[str, Any]:
        """Adjust product stock"""
        return self._api_call('POST', '/api/v1/inventory/adjust', json={
            'productId': product_id,
            'quantity': quantity,
            'reason': reason
        })
    
    # Reports
    def generate_sales_report(self, start_date: str, end_date: str, format: str = 'pdf') -> bytes:
        """Generate sales report"""
        self._ensure_valid_token()
        
        response = self.session.get(
            f"{self.base_url}/api/v1/reports/sales",
            params={
                'startDate': start_date,
                'endDate': end_date,
                'format': format
            }
        )
        response.raise_for_status()
        
        return response.content

# Usage example
def main():
    client = PdvApiClient('https://api.pdvsystem.com')
    
    try:
        # Login
        auth_result = client.login('admin@pdvsystem.com', 'password123')
        print(f"Logged in as: {auth_result['user']['nome']}")
        
        # Get products
        products = client.get_products(page=1, page_size=10)
        print(f"Found {len(products.get('items', []))} products")
        
        # Create a new product
        new_product = client.create_product({
            'nome': 'New Product',
            'descricao': 'Product description',
            'preco': 29.99,
            'codigoBarras': '1234567890123'
        })
        print(f"Created product: {new_product['id']}")
        
        # Generate sales report
        report_data = client.generate_sales_report('2024-01-01', '2024-01-31', 'pdf')
        with open('sales-report.pdf', 'wb') as f:
            f.write(report_data)
        print("Sales report saved to sales-report.pdf")
        
    except Exception as e:
        print(f"Error: {e}")
    finally:
        client.logout()

if __name__ == '__main__':
    main()
```

## cURL Examples

### Authentication

```bash
# Login
curl -X POST "https://api.pdvsystem.com/api/v1/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "login": "admin@pdvsystem.com",
    "password": "password123"
  }'

# Response:
# {
#   "success": true,
#   "message": "Login realizado com sucesso",
#   "data": {
#     "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
#     "refreshToken": "def502004a8c...",
#     "expiresIn": 3600,
#     "user": { ... }
#   }
# }

# Store tokens for subsequent requests
ACCESS_TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
REFRESH_TOKEN="def502004a8c..."

# Refresh token
curl -X POST "https://api.pdvsystem.com/api/v1/auth/refresh" \
  -H "Content-Type: application/json" \
  -d "{\"refreshToken\": \"$REFRESH_TOKEN\"}"

# Get current user info
curl -X GET "https://api.pdvsystem.com/api/v1/auth/me" \
  -H "Authorization: Bearer $ACCESS_TOKEN"

# Logout
curl -X POST "https://api.pdvsystem.com/api/v1/auth/logout" \
  -H "Content-Type: application/json" \
  -d "{\"refreshToken\": \"$REFRESH_TOKEN\"}"
```

### Product Operations

```bash
# Get products (paginated)
curl -X GET "https://api.pdvsystem.com/api/v1/produto/paginated?page=1&pageSize=20" \
  -H "Authorization: Bearer $ACCESS_TOKEN"

# Create product
curl -X POST "https://api.pdvsystem.com/api/v1/produto" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "New Product",
    "descricao": "Product description",
    "preco": 29.99,
    "codigoBarras": "1234567890123"
  }'

# Update product
curl -X PUT "https://api.pdvsystem.com/api/v1/produto/123e4567-e89b-12d3-a456-426614174000" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Updated Product Name",
    "preco": 39.99
  }'

# Delete product
curl -X DELETE "https://api.pdvsystem.com/api/v1/produto/123e4567-e89b-12d3-a456-426614174000" \
  -H "Authorization: Bearer $ACCESS_TOKEN"
```

### Inventory Operations

```bash
# Get stock movements
curl -X GET "https://api.pdvsystem.com/api/v1/inventory/movements" \
  -H "Authorization: Bearer $ACCESS_TOKEN"

# Get stock movements for specific product
curl -X GET "https://api.pdvsystem.com/api/v1/inventory/movements?productId=123e4567-e89b-12d3-a456-426614174000" \
  -H "Authorization: Bearer $ACCESS_TOKEN"

# Adjust stock
curl -X POST "https://api.pdvsystem.com/api/v1/inventory/adjust" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "productId": "123e4567-e89b-12d3-a456-426614174000",
    "quantity": 10,
    "reason": "Stock replenishment"
  }'

# Get stock alerts
curl -X GET "https://api.pdvsystem.com/api/v1/inventory/alerts" \
  -H "Authorization: Bearer $ACCESS_TOKEN"
```

### Reports

```bash
# Generate sales report (PDF)
curl -X GET "https://api.pdvsystem.com/api/v1/reports/sales?startDate=2024-01-01&endDate=2024-01-31&format=pdf" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -o sales-report.pdf

# Generate inventory report (Excel)
curl -X GET "https://api.pdvsystem.com/api/v1/reports/inventory?format=excel" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -o inventory-report.xlsx
```

## Error Handling

### Common HTTP Status Codes

- **200 OK** - Request successful
- **400 Bad Request** - Invalid request data or validation errors
- **401 Unauthorized** - Invalid or missing authentication token
- **403 Forbidden** - Valid token but insufficient permissions
- **404 Not Found** - Resource not found
- **409 Conflict** - Resource conflict (e.g., duplicate data)
- **422 Unprocessable Entity** - Validation errors
- **429 Too Many Requests** - Rate limit exceeded
- **500 Internal Server Error** - Server error

### Error Response Format

```json
{
  "success": false,
  "message": "Error description",
  "errors": [
    "Detailed error message 1",
    "Detailed error message 2"
  ]
}
```

### Validation Error Response

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": [
    "Nome é obrigatório",
    "Preço deve ser maior que zero",
    "Código de barras deve ter entre 8 e 14 dígitos"
  ]
}
```

## Rate Limiting

The API implements rate limiting to prevent abuse:

- **1000 requests per hour** per IP address
- **100 requests per minute** per authenticated user
- **10 requests per minute** for authentication endpoints per IP

### Rate Limit Headers

```
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 999
X-RateLimit-Reset: 1640995200
```

### Handling Rate Limits

```javascript
// Example: Exponential backoff for rate limit handling
async function apiCallWithRetry(apiCall, maxRetries = 3) {
  for (let attempt = 0; attempt < maxRetries; attempt++) {
    try {
      return await apiCall();
    } catch (error) {
      if (error.status === 429 && attempt < maxRetries - 1) {
        const delay = Math.pow(2, attempt) * 1000; // Exponential backoff
        await new Promise(resolve => setTimeout(resolve, delay));
        continue;
      }
      throw error;
    }
  }
}
```

## Postman Collection

A comprehensive Postman collection is available for testing the API. Import the collection and environment files:

1. **Collection File**: `PDV-Control-System-API.postman_collection.json`
2. **Environment File**: `PDV-API-Environment.postman_environment.json`

### Environment Variables

Set these variables in your Postman environment:

- `baseUrl`: API base URL (e.g., `https://api.pdvsystem.com`)
- `accessToken`: Will be set automatically after login
- `refreshToken`: Will be set automatically after login

### Pre-request Scripts

The collection includes pre-request scripts for automatic token management:

```javascript
// Auto-refresh token if expired
const expiresAt = pm.environment.get('tokenExpiresAt');
if (expiresAt && new Date() >= new Date(expiresAt)) {
    // Refresh token logic here
}
```

This comprehensive guide should help developers integrate with the PDV Control System API using their preferred programming language and tools.