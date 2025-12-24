# Single Platform Architecture

## Vision

A unified, cloud-ready platform that replaces the legacy Excel/Word/SAP silos with a single, integrated system for product configuration, document generation, and order management.

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                    Web Application Layer                     │
│  (React/Angular SPA or Blazor Server)                       │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────────────────────┴──────────────────────────────────────┐
│                    API Gateway / BFF                        │
│  (Rate Limiting, Authentication, Request Routing)           │
└──────────────────────┬──────────────────────────────────────┘
                       │
        ┌──────────────┼──────────────┐
        │              │              │
┌───────▼──────┐ ┌─────▼──────┐ ┌────▼──────┐
│ Configurator │ │  Document  │ │   Order   │
│     API      │ │  Generator │ │ Management│
│              │ │    API     │ │    API    │
└───────┬──────┘ └─────┬──────┘ └────┬──────┘
        │              │              │
        └──────────────┼──────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                    Business Logic Layer                     │
│  (Domain Services, Validation, Pricing Engine)              │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                    Data Access Layer                         │
│  (Entity Framework Core, Repository Pattern)               │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                    Data Storage                              │
│  (SQL Server / PostgreSQL for configs, orders)              │
│  (Blob Storage for PDFs)                                    │
│  (Redis for caching)                                        │
└─────────────────────────────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                    External Integrations                     │
│  (SAP ERP via API / EDI)                                    │
│  (Email Service for document delivery)                      │
│  (Notification Service for order status)                    │
└─────────────────────────────────────────────────────────────┘
```

## Core Components

### 1. Configuration Service

**Responsibilities:**
- Product configuration validation
- Pricing calculation with business rules
- Configuration ID generation and management
- Configuration history and versioning

**Technology:**
- ASP.NET Core Web API
- Domain-driven design
- In-memory caching for pricing rules

### 2. Document Generation Service

**Responsibilities:**
- PDF generation (Sales Sheets, Plant Instructions, Quotes)
- Document template management
- Document versioning
- Batch document generation

**Technology:**
- QuestPDF or iTextSharp
- Template engine for dynamic content
- Blob storage for generated documents

### 3. Order Management Service

**Responsibilities:**
- Order creation and validation
- Order status tracking
- ERP integration (SAP)
- Order history and audit trail

**Technology:**
- ASP.NET Core Web API
- Message queue for async processing
- Integration adapters for ERP systems

### 4. User Management & Authentication

**Responsibilities:**
- User authentication (Azure AD / OAuth2)
- Role-based access control
- Audit logging
- Session management

**Technology:**
- ASP.NET Core Identity
- JWT tokens
- OAuth2 / OpenID Connect

### 5. Data Persistence

**Responsibilities:**
- Configuration storage
- Order storage
- User data
- Audit logs

**Technology:**
- SQL Server or PostgreSQL
- Entity Framework Core
- Repository pattern
- Unit of Work pattern

## Integration Points

### ERP Integration (SAP)

**Approach:**
- REST API integration (SAP OData services)
- EDI for high-volume orders
- Message queue for reliable delivery
- Retry logic with exponential backoff

**Data Flow:**
```
Order Management API → Message Queue → ERP Adapter → SAP
                                    ↓
                              Status Updates ← SAP
```

### Email Service

**Approach:**
- SMTP service (SendGrid, AWS SES)
- Template-based emails
- Attachment support (PDFs)
- Delivery tracking

### Notification Service

**Approach:**
- Real-time notifications (SignalR)
- Email notifications
- SMS notifications (optional)
- In-app notification center

## Data Model

### Core Entities

**Configuration**
- ConfigurationId (PK)
- ProductType
- Dimensions
- Material
- Options
- Pricing details
- CreatedBy, CreatedAt
- Version

**Order**
- OrderId (PK)
- ConfigurationId (FK)
- CustomerId (FK)
- Status
- RequestedShipDate
- TotalPrice
- ERPOrderId
- CreatedAt, UpdatedAt

**Customer**
- CustomerId (PK)
- Name
- Contact information
- Credit terms
- Shipping addresses

**User**
- UserId (PK)
- Email
- Role
- Department
- LastLogin

## Security Architecture

### Authentication
- OAuth2 / OpenID Connect
- Azure AD integration
- Multi-factor authentication support

### Authorization
- Role-based access control (RBAC)
- Sales Rep, Order Entry, Manager, Admin roles
- Resource-level permissions

### Data Protection
- Encryption at rest (database)
- Encryption in transit (HTTPS/TLS)
- PII data masking
- Audit logging for sensitive operations

## Scalability & Performance

### Horizontal Scaling
- Stateless API design
- Load balancer for API instances
- Database read replicas
- Caching layer (Redis)

### Performance Optimization
- Response caching for pricing rules
- Async/await for I/O operations
- Database query optimization
- CDN for static assets

### Monitoring & Observability
- Application Insights / New Relic
- Structured logging (Serilog)
- Health check endpoints
- Performance metrics (APM)

## Deployment Architecture

### Development Environment
- Local development with Docker Compose
- SQL Server LocalDB
- Local file storage

### Staging Environment
- Azure App Service (or AWS ECS)
- Azure SQL Database
- Azure Blob Storage
- Azure Redis Cache

### Production Environment
- Azure App Service (multiple instances)
- Azure SQL Database (Premium tier)
- Azure Blob Storage (Geo-redundant)
- Azure Redis Cache (Premium tier)
- Azure Application Gateway (load balancer)

## Migration Benefits

1. **Elimination of Manual Processes**: Automated configuration-to-order flow
2. **Error Reduction**: Validation and business rules enforced programmatically
3. **Faster Turnaround**: Real-time configuration and instant document generation
4. **Audit Trail**: Complete history of all configurations and orders
5. **Scalability**: Cloud-native architecture supports growth
6. **Integration**: Seamless ERP integration eliminates re-keying
7. **User Experience**: Modern web interface replaces Excel/Word workflows

## Technology Stack Summary

- **Frontend**: React/Angular or Blazor Server
- **Backend**: ASP.NET Core 8.0
- **Database**: SQL Server / PostgreSQL
- **Caching**: Redis
- **Message Queue**: Azure Service Bus / RabbitMQ
- **PDF Generation**: QuestPDF
- **Authentication**: Azure AD / IdentityServer
- **Hosting**: Azure App Service / AWS ECS
- **Monitoring**: Application Insights / New Relic

