# Cloud Cost Optimizer MCP

A comprehensive cloud cost optimization platform that detects waste, analyzes resource utilization, and provides actionable recommendations to reduce cloud spending across AWS, Azure, and GCP.

## 🌟 Features

### Backend (ASP.NET Core 7.0 Web API)

- **Comprehensive Resource Tracking**: Monitor 50+ cloud resources across multiple providers
- **Intelligent Waste Detection**: 
  - Idle Resources (CPU < 5%, Memory < 10% for 7+ days)
  - Oversized Instances (consistent low utilization)
  - Unattached Storage Volumes
  - Stopped Resources still incurring costs
  - Zombie Resources (no activity for 30+ days)
  - Redundant Backups

- **Smart Recommendation Engine**:
  - Priority-based recommendations (High/Medium/Low)
  - ROI calculation for each recommendation
  - Implementation effort estimation
  - Impact analysis with percentage savings

- **Advanced Analytics**:
  - Cost breakdown by provider and resource type
  - Historical cost trends (6 months)
  - Waste analysis by category
  - Top wasteful resources identification

### Frontend (Modern HTML5/CSS3/JavaScript)

- **Interactive Dashboard** with real-time metrics
- **Visual Analytics** using Chart.js:
  - Pie chart for waste breakdown
  - Bar chart for cost by provider
  - Line chart for cost trends
  - Doughnut chart for resource distribution

- **Key Metrics Cards**:
  - Total Monthly Cost
  - Total Waste Detected
  - Potential Monthly/Annual Savings
  - Savings Percentage
  - High Priority Recommendations

- **Advanced Features**:
  - Filter by provider and resource type
  - Search functionality
  - Sortable tables
  - Responsive design for mobile devices
  - One-click data refresh

## 📋 Prerequisites

- .NET 7.0 SDK or later
- Visual Studio 2022 or VS Code
- Modern web browser (Chrome, Firefox, Edge, Safari)

## 🚀 Getting Started

### 1. Clone or Extract the Project

```bash
cd CloudOptimizerMCP
```

### 2. Build the Backend

```bash
cd CloudCostOptimizer
dotnet restore
dotnet build
```

### 3. Run the API Server

```bash
dotnet run
```

The API will start on `http://localhost:5010` (or the port specified in `launchSettings.json`)

### 4. Open the Frontend

Open `CloudCostOptimizerUI/index.html` in your web browser, or use a local web server:

```bash
# Using Python
cd CloudCostOptimizerUI
python -m http.server 8000

# Using Node.js
npx http-server -p 8000
```

Then navigate to `http://localhost:8000`

## 📊 API Endpoints

### Legacy Endpoints (Backward Compatible)

- `GET /api/cost/costs` - Get basic cost information by provider
- `GET /api/cost/idle-resources` - Get idle resources list
- `GET /api/cost/recommendations` - Get simple recommendations list

### Enhanced Endpoints

#### Summary & Analytics
- `GET /api/cost/summary` - Get comprehensive cost summary with all metrics
- `GET /api/cost/waste-breakdown` - Get waste analysis by category
- `GET /api/cost/waste-by-provider` - Get waste breakdown by cloud provider
- `GET /api/cost/potential-savings` - Get total potential savings

#### Resources
- `GET /api/cost/resources` - Get all cloud resources
- `GET /api/cost/resources/provider/{provider}` - Filter by provider (AWS/Azure/GCP)
- `GET /api/cost/resources/type/{type}` - Filter by resource type
- `GET /api/cost/resources/state/{state}` - Filter by resource state
- `GET /api/cost/top-wasteful?count=10` - Get top wasteful resources

#### Recommendations
- `GET /api/cost/recommendations/detailed` - Get detailed recommendations with metadata
- `GET /api/cost/recommendations/high-priority` - Get only high-priority recommendations

#### Trends
- `GET /api/cost/trends` - Get cost trends for all providers (6 months)
- `GET /api/cost/trends/provider/{provider}` - Get trends for specific provider

#### Utility
- `POST /api/cost/refresh` - Refresh mock data (generates new dataset)

## 🏗️ Architecture

```
CloudOptimizerMCP/
├── CloudCostOptimizer/              # Backend API
│   ├── Controllers/
│   │   └── CostController.cs        # API endpoints
│   ├── Models/
│   │   ├── CloudCost.cs             # Legacy cost model
│   │   ├── IdleResource.cs          # Legacy idle resource model
│   │   ├── ResourceType.cs          # Enums for resource types and states
│   │   ├── CloudResource.cs         # Comprehensive resource model
│   │   ├── Recommendation.cs        # Recommendation model with priority
│   │   └── WasteAnalysis.cs         # Waste analysis and summary models
│   ├── Services/
│   │   ├── CostService.cs           # Main service orchestrator
│   │   ├── DataGenerator.cs         # Realistic mock data generator
│   │   ├── WasteAnalyzer.cs         # Waste detection algorithms
│   │   └── RecommendationEngine.cs  # Recommendation generation logic
│   ├── Tools/
│   │   └── CostTools.cs             # MCP tools integration
│   └── Program.cs                   # Application entry point
└── CloudCostOptimizerUI/            # Frontend Dashboard
    └── index.html                   # Single-page application
```

## 💡 Key Algorithms

### Waste Detection

1. **Idle Resources**: `CPU < 5% AND Memory < 10% AND DaysIdle >= 7`
2. **Oversized Instances**: `CPU < 20% AND Memory < 30% AND Type = VM`
3. **Unattached Volumes**: `Type = Storage AND DaysIdle > 30`
4. **Stopped Resources**: `State = Stopped`
5. **Zombie Resources**: `LastAccessDate > 30 days ago`

### Savings Calculation

- **Idle Resources**: 85% of monthly cost
- **Oversized Instances**: 35% of monthly cost (from downsizing)
- **Unattached Volumes**: 100% of monthly cost
- **Stopped Resources**: 25% of monthly cost
- **Zombie Resources**: 90% of monthly cost
- **Reserved Instances**: 30% savings for stable workloads
- **Scheduling**: 50% savings for non-production resources

### Priority Scoring

- **High Priority**: Potential savings > $1000/month OR waste category is critical
- **Medium Priority**: Potential savings $500-$1000/month
- **Low Priority**: Potential savings < $500/month

## 🎨 Customization

### Modify Mock Data

Edit `Services/DataGenerator.cs` to:
- Change the number of resources generated
- Adjust cost ranges
- Modify resource states distribution
- Add new cloud providers or regions

### Adjust Waste Thresholds

Edit `Services/WasteAnalyzer.cs` to:
- Change CPU/Memory thresholds for idle detection
- Modify days idle criteria
- Adjust oversized instance detection logic

### Customize Recommendations

Edit `Services/RecommendationEngine.cs` to:
- Add new recommendation types
- Modify priority scoring logic
- Change savings calculation formulas
- Add custom recommendation categories

### Update UI Theme

Edit the `<style>` section in `CloudCostOptimizerUI/index.html` to:
- Change color scheme
- Modify card layouts
- Adjust chart colors
- Update responsive breakpoints

## 🔧 Configuration

### API Port Configuration

Edit `Properties/launchSettings.json`:

```json
{
  "applicationUrl": "http://localhost:5010"
}
```

### CORS Configuration

Edit `Program.cs` to modify allowed origins:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("http://localhost:8000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
```

### Frontend API URL

Edit `CloudCostOptimizerUI/index.html`:

```javascript
const API_BASE = 'http://localhost:5010/api/cost';
```

## 📈 Sample Data

The application generates realistic mock data including:

- **53 Cloud Resources** across AWS (20), Azure (18), and GCP (15)
- **Multiple Resource Types**: VMs, Databases, Storage, Load Balancers, Containers, Functions
- **Various States**: Running, Idle, Stopped, Oversized, Unattached, Zombie
- **Realistic Costs**: Based on actual cloud provider pricing
- **6 Months of Historical Data**: Weekly cost trends for trend analysis

## 🚀 Production Deployment

### Backend Deployment

1. **Publish the application**:
```bash
dotnet publish -c Release -o ./publish
```

2. **Deploy to**:
   - Azure App Service
   - AWS Elastic Beanstalk
   - Docker container
   - IIS Server

### Frontend Deployment

1. **Update API URL** in `index.html` to production API endpoint
2. **Deploy to**:
   - Azure Static Web Apps
   - AWS S3 + CloudFront
   - Netlify
   - Vercel
   - Any static hosting service

## 🔌 Integration with Real Cloud Providers

To integrate with actual cloud providers, replace the mock `DataGenerator` with real API calls:

### AWS Integration
```csharp
// Install AWS SDK
// dotnet add package AWSSDK.CostExplorer
// dotnet add package AWSSDK.EC2

// Use AWS Cost Explorer API and EC2 API
```

### Azure Integration
```csharp
// Install Azure SDK
// dotnet add package Azure.ResourceManager.CostManagement
// dotnet add package Azure.ResourceManager.Compute

// Use Azure Cost Management API and Compute API
```

### GCP Integration
```csharp
// Install Google Cloud SDK
// dotnet add package Google.Cloud.Billing.V1
// dotnet add package Google.Cloud.Compute.V1

// Use Google Cloud Billing API and Compute API
```

## 🧪 Testing

### Test API Endpoints

Using curl:
```bash
# Get summary
curl http://localhost:5010/api/cost/summary

# Get recommendations
curl http://localhost:5010/api/cost/recommendations/detailed

# Refresh data
curl -X POST http://localhost:5010/api/cost/refresh
```

Using Swagger UI:
Navigate to `http://localhost:5010/swagger` when the API is running

## 📝 License

This project is provided as-is for educational and demonstration purposes.

## 🤝 Contributing

Contributions are welcome! Areas for improvement:
- Real cloud provider integration
- Machine learning for better predictions
- Multi-tenancy support
- User authentication and authorization
- Email notifications for recommendations
- Export functionality (PDF, Excel)
- Historical data persistence (database)

## 📧 Support

For questions or issues, please refer to the inline code documentation or create an issue in the repository.

## 🎯 Roadmap

- [ ] Real-time cloud provider integration
- [ ] Machine learning-based anomaly detection
- [ ] Automated remediation workflows
- [ ] Multi-cloud cost comparison
- [ ] Budget alerts and notifications
- [ ] Team collaboration features
- [ ] Mobile application
- [ ] Advanced reporting and exports

---

**Built with ❤️ using ASP.NET Core 7.0, Chart.js, and modern web technologies**