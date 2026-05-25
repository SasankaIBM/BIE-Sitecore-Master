# CloudOptimizer Requirements Schema (JSON-LD)

A comprehensive JSON-LD schema documenting the **actual implementation** of the Cloud Cost Optimizer MCP application located at `C:/Users/AsuriSruthi/Desktop/CloudOptimizerMCP`.

## Overview

This schema provides a complete, machine-readable representation of the CloudOptimizerMCP codebase, including:
- **4 Data Models** with full property definitions
- **4 Enumerations** for type safety
- **4 Service Components** with responsibilities
- **5 Waste Detection Algorithms** with criteria and savings calculations
- **7 Recommendation Rules** with priority logic
- **17 API Endpoints** with parameters and return types
- **3 MCP Tools** for AI assistant integration
- **14 Requirements** (7 Functional, 5 Non-Functional, 2 Security)
- **11 Future Enhancements** from the roadmap

## Schema File

**File**: `cloudoptimizer-requirements-schema.jsonld`  
**Lines**: 1,179  
**Format**: JSON-LD 1.1  
**Validation**: ✅ 83 checks passed, 0 failures, 0 warnings

## Key Components Documented

### 1. Data Models

#### CloudResource
Complete model with 19 properties representing cloud resources across AWS, Azure, and GCP:
- Resource identification (ID, Name, Provider, Type, State, Region)
- Cost metrics (MonthlyCost, EstimatedWaste, PotentialSavings)
- Utilization metrics (CpuUsage, MemoryUsage, StorageUsage)
- Temporal data (CreatedDate, LastAccessDate, DaysIdle)
- Sizing information (InstanceSize, RecommendedSize)
- Metadata (Tags, IsProduction)

**Source**: `Models/CloudResource.cs`

#### Recommendation
Optimization recommendation model with 13 properties:
- Identification (Id, Title, Description)
- Prioritization (Priority, Category)
- Financial impact (PotentialMonthlySavings, AnnualSavings)
- Implementation details (ImplementationEffort, ImpactPercentage, ActionRequired)
- Affected resources tracking (AffectedResources, ResourceCount, Provider)
- ROI calculation method

**Source**: `Models/Recommendation.cs`

#### WasteAnalysis
Waste breakdown by category with 6 properties:
- Category classification
- Financial metrics (TotalWaste)
- Resource counting
- Percentage calculations
- Descriptive information

**Source**: `Models/WasteAnalysis.cs`

#### CostSummary
Comprehensive cost summary with 11 properties:
- Total cost metrics
- Waste and savings calculations
- Resource counts
- Breakdown by provider and type
- High-priority recommendation count

**Source**: `Models/WasteAnalysis.cs`

### 2. Enumerations

All enums from `Models/ResourceType.cs`:

- **ResourceType**: 10 values (VirtualMachine, Database, Storage, LoadBalancer, Network, Container, Function, Cache, Queue, Other)
- **ResourceState**: 6 values (Running, Stopped, Idle, Oversized, Unattached, Zombie)
- **WasteCategory**: 7 values (IdleResources, OversizedInstances, UnattachedVolumes, StoppedResources, ZombieResources, RedundantBackups, UnusedReservations)
- **RecommendationPriority**: 3 values (High, Medium, Low)

### 3. Service Components

#### CostService
Main orchestrator coordinating all cost management operations.
**Source**: `Services/CostService.cs`

#### DataGenerator
Generates realistic mock data for 53 cloud resources:
- AWS: 20 resources
- Azure: 18 resources
- GCP: 15 resources
- 6 months of historical trend data

**Source**: `Services/DataGenerator.cs`

#### WasteAnalyzer
Implements 5 waste detection algorithms with specific criteria.
**Source**: `Services/WasteAnalyzer.cs`

#### RecommendationEngine
Generates 7 types of prioritized recommendations with ROI calculations.
**Source**: `Services/RecommendationEngine.cs`

### 4. Waste Detection Algorithms

Each algorithm documented with:
- Detection criteria
- Savings calculation formula
- Implementation method reference

| Algorithm | Criteria | Savings Rate |
|-----------|----------|--------------|
| Idle Resources | CPU < 5% AND Memory < 10% AND DaysIdle >= 7 | 85% |
| Oversized Instances | CPU < 20% AND Memory < 30% AND Type = VM | 35% |
| Unattached Volumes | Type = Storage AND DaysIdle > 30 | 100% |
| Stopped Resources | State = Stopped | 25% |
| Zombie Resources | LastAccessDate > 30 days ago | 90% |

**Source**: `Services/WasteAnalyzer.cs`

### 5. Recommendation Rules

Each rule documented with:
- Priority logic
- Implementation effort (1-5 scale)
- Required actions
- Savings calculations

| Rule | Priority Logic | Effort | Savings |
|------|---------------|--------|---------|
| Terminate Idle Resources | High if > $1000 | 2 | 85% |
| Downsize Oversized Instances | High if > $800 | 3 | 35% |
| Delete Unattached Volumes | Always High | 1 | 100% |
| Review Stopped Resources | Medium | 2 | 25% |
| Clean Up Zombie Resources | High | 2 | 90% |
| Purchase Reserved Instances | High if > $1000 | 3 | 30% |
| Implement Resource Scheduling | High if > $500 | 3 | 50% |

**Source**: `Services/RecommendationEngine.cs`

### 6. API Endpoints

All 17 REST API endpoints documented with:
- HTTP method and path
- Controller and service mapping
- Parameters and return types
- Model usage

#### Legacy Endpoints (Backward Compatible)
- `GET /api/cost/costs` - Basic cost information
- `GET /api/cost/idle-resources` - Idle resources list
- `GET /api/cost/recommendations` - Simple recommendations

#### Enhanced Endpoints
- `GET /api/cost/summary` - Comprehensive cost summary
- `GET /api/cost/waste-breakdown` - Waste by category
- `GET /api/cost/resources` - All resources
- `GET /api/cost/resources/provider/{provider}` - Filter by provider
- `GET /api/cost/resources/type/{type}` - Filter by type
- `GET /api/cost/resources/state/{state}` - Filter by state
- `GET /api/cost/recommendations/detailed` - Detailed recommendations
- `GET /api/cost/recommendations/high-priority` - High-priority only
- `GET /api/cost/trends` - 6-month cost trends
- `GET /api/cost/trends/provider/{provider}` - Provider-specific trends
- `GET /api/cost/top-wasteful?count=10` - Top wasteful resources
- `GET /api/cost/waste-by-provider` - Waste by provider
- `GET /api/cost/potential-savings` - Total savings potential
- `POST /api/cost/refresh` - Refresh mock data

**Source**: `Controllers/CostController.cs`

### 7. MCP Integration

Model Context Protocol tools for AI assistant integration:
- `GetCloudCosts` - Retrieve all cloud costs
- `DetectIdleResources` - Detect idle resources
- `GetRecommendations` - Get optimization recommendations

**Source**: `Tools/CostTools.cs`

### 8. Requirements

#### Functional Requirements (7)
- **FR-001**: Multi-Cloud Resource Monitoring (53 resources across AWS, Azure, GCP)
- **FR-002**: Intelligent Waste Detection (6 categories with specific algorithms)
- **FR-003**: Smart Recommendation Engine (7 recommendation types with ROI)
- **FR-004**: Comprehensive Analytics Dashboard (Charts, filters, real-time metrics)
- **FR-005**: Historical Trend Analysis (6 months of data)
- **FR-006**: Data Refresh Capability (On-demand mock data regeneration)
- **FR-007**: MCP Integration for AI Assistants (3 tools)

#### Non-Functional Requirements (5)
- **NFR-001**: Scalability (53 resources, expandable architecture)
- **NFR-002**: Performance (In-memory processing, sub-second responses)
- **NFR-003**: Maintainability (Clean MVC architecture)
- **NFR-004**: Usability (Modern UI with Chart.js visualizations)
- **NFR-005**: Cross-Platform Compatibility (.NET 7.0, browser-agnostic)

#### Security Requirements (2)
- **SEC-001**: CORS Configuration (AllowAll policy for development)
- **SEC-002**: API Documentation (Swagger/OpenAPI at /swagger)

All requirements marked as **"Implemented"** with links to:
- Service components that implement them
- Data models they use
- API endpoints they expose
- Algorithms they employ

## Technical Stack

### Backend
- **Framework**: ASP.NET Core 7.0
- **Language**: C# 11
- **Architecture**: MVC Pattern
- **Dependencies**: 
  - Microsoft.AspNetCore.OpenApi
  - Swashbuckle.AspNetCore
  - ModelContextProtocol.Core
  - ModelContextProtocol

### Frontend
- **Technologies**: HTML5, CSS3, JavaScript ES6+
- **Libraries**: Chart.js
- **Architecture**: Single Page Application

### Integration
- **Protocol**: Model Context Protocol (MCP)
- **Purpose**: AI Assistant Integration

## Deployment Configuration

- **API Port**: 5010
- **CORS Policy**: AllowAll (development)
- **Swagger**: Enabled at /swagger
- **Environment**: Development

## Usage

### Validate the Schema

```bash
node validate-cloudoptimizer-schema.js
```

Expected output:
```
✅ Validation PASSED - Schema accurately represents CloudOptimizerMCP
✓ Passed: 83
✗ Failed: 0
⚠ Warnings: 0
```

### Query the Schema

```javascript
const fs = require('fs');
const schema = JSON.parse(fs.readFileSync('cloudoptimizer-requirements-schema.jsonld', 'utf8'));
const app = schema['@graph'][0];

// Get all API endpoints
const endpoints = app.apiEndpoints.map(e => ({
  method: e.method,
  path: e.path,
  description: e.description
}));

// Get all waste detection algorithms
const algorithms = app.wasteDetectionAlgorithms.map(a => ({
  name: a.name,
  criteria: a.criteria,
  savings: a.savingsCalculation
}));

// Get all requirements
const requirements = app.requirements.map(r => ({
  id: r.requirementId,
  name: r.name,
  type: r['@type'],
  status: r.status
}));
```

## Traceability

The schema provides complete traceability from requirements to implementation:

1. **Requirements → Services**: Each requirement lists the service components that implement it
2. **Requirements → Models**: Each requirement lists the data models it uses
3. **Requirements → APIs**: Each requirement lists the API endpoints that expose it
4. **Requirements → Algorithms**: Functional requirements link to specific algorithms
5. **Services → Source Files**: Each service component references its source file
6. **Models → Source Files**: Each model references its source file
7. **Algorithms → Methods**: Each algorithm references the implementing method

## Benefits

1. **Accurate Documentation**: Schema generated from actual codebase analysis
2. **Machine-Readable**: JSON-LD format enables automated processing
3. **Complete Coverage**: All models, services, APIs, and algorithms documented
4. **Validated**: 83 validation checks ensure accuracy
5. **Traceable**: Clear links between requirements and implementation
6. **Maintainable**: Easy to update as code evolves
7. **Queryable**: Standard JSON format for easy querying

## Comparison with Generic Schema

This schema differs from the generic `cloud-waste-requirements-schema.jsonld` in that it:

- Documents the **actual implementation** rather than theoretical requirements
- Includes **real source file references** (e.g., `Models/CloudResource.cs`)
- Maps **specific methods** in service classes (e.g., `AnalyzeIdleResources`)
- Documents **actual API endpoints** from the controller
- Includes **real data** (53 resources, 20 AWS, 18 Azure, 15 GCP)
- References **actual algorithms** with exact criteria from the code
- Links **requirements to implementation** with specific service/model/API references
- Includes **MCP integration** details from the Tools directory
- Documents **actual enumerations** from ResourceType.cs

## Future Enhancements

The schema documents 11 planned enhancements from the project roadmap:

1. Real-time cloud provider integration (AWS SDK, Azure SDK, GCP SDK)
2. Machine learning-based anomaly detection
3. Automated remediation workflows
4. Multi-cloud cost comparison
5. Budget alerts and notifications
6. Team collaboration features
7. Mobile application
8. Advanced reporting and exports (PDF, Excel)
9. Historical data persistence (database)
10. User authentication and authorization
11. Multi-tenancy support

## Files Generated

1. **cloudoptimizer-requirements-schema.jsonld** (1,179 lines)
   - Complete JSON-LD schema of the CloudOptimizerMCP implementation

2. **validate-cloudoptimizer-schema.js** (330 lines)
   - Validation script with 83 checks
   - Verifies schema accuracy against expected implementation

3. **CLOUDOPTIMIZER-SCHEMA-README.md** (This file)
   - Comprehensive documentation of the schema

## Validation Results

```
Data Models: 4
Enumerations: 4
Service Components: 4
Waste Detection Algorithms: 5
Recommendation Rules: 7
API Endpoints: 17
MCP Tools: 3
Requirements: 14
Future Enhancements: 11

✅ All components validated successfully
```

## Source Code Location

**CloudOptimizerMCP Project**: `C:/Users/AsuriSruthi/Desktop/CloudOptimizerMCP`

### Key Files Analyzed
- `CloudCostOptimizer/Program.cs` - Application entry point
- `Controllers/CostController.cs` - 17 API endpoints
- `Models/CloudResource.cs` - Main resource model
- `Models/Recommendation.cs` - Recommendation model
- `Models/WasteAnalysis.cs` - Analysis models
- `Models/ResourceType.cs` - All enumerations
- `Services/CostService.cs` - Main orchestrator
- `Services/DataGenerator.cs` - Mock data generation
- `Services/WasteAnalyzer.cs` - Waste detection algorithms
- `Services/RecommendationEngine.cs` - Recommendation rules
- `Tools/CostTools.cs` - MCP integration
- `README.md` - Project documentation

## License

This schema documentation is provided as-is for the CloudOptimizerMCP project.

## Contact

For questions about the schema or the CloudOptimizerMCP implementation, refer to the project's README.md or source code documentation.

---

**Generated**: 2026-05-21  
**Schema Version**: 1.0.0  
**CloudOptimizerMCP Version**: 1.0.0  
**Validation Status**: ✅ PASSED (83/83 checks)