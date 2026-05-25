# Cloud Cost Optimizer - Deployment Guide

This guide provides step-by-step instructions for deploying the Cloud Cost Optimizer application to various environments.

## 📋 Table of Contents

1. [Local Development Setup](#local-development-setup)
2. [Production Deployment Options](#production-deployment-options)
3. [Azure Deployment](#azure-deployment)
4. [AWS Deployment](#aws-deployment)
5. [Docker Deployment](#docker-deployment)
6. [Configuration Management](#configuration-management)
7. [Monitoring & Logging](#monitoring--logging)
8. [Troubleshooting](#troubleshooting)

---

## 🖥️ Local Development Setup

### Prerequisites

- .NET 7.0 SDK: [Download](https://dotnet.microsoft.com/download/dotnet/7.0)
- Visual Studio 2022 or VS Code
- Git (optional)

### Step 1: Setup Backend

```bash
# Navigate to backend directory
cd CloudOptimizerMCP/CloudCostOptimizer

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

The API will be available at: `http://localhost:5010`

### Step 2: Setup Frontend

```bash
# Navigate to frontend directory
cd CloudOptimizerMCP/CloudCostOptimizerUI

# Option 1: Open directly in browser
# Simply open index.html in your browser

# Option 2: Use a local web server (recommended)
# Using Python
python -m http.server 8000

# Using Node.js
npx http-server -p 8000

# Using .NET
dotnet tool install --global dotnet-serve
dotnet serve -p 8000
```

The UI will be available at: `http://localhost:8000`

### Step 3: Verify Setup

1. Open browser to `http://localhost:8000`
2. Dashboard should load with metrics and charts
3. Check browser console for any errors
4. Verify API is responding at `http://localhost:5010/swagger`

---

## 🚀 Production Deployment Options

### Option 1: Azure App Service + Azure Static Web Apps

**Best for**: Microsoft Azure customers, easy scaling, managed infrastructure

**Pros**:
- Fully managed platform
- Auto-scaling
- Built-in SSL/TLS
- Easy CI/CD integration

**Cons**:
- Azure-specific
- Costs can add up with scale

### Option 2: AWS Elastic Beanstalk + S3/CloudFront

**Best for**: AWS customers, global distribution, cost-effective

**Pros**:
- AWS ecosystem integration
- Global CDN with CloudFront
- Pay-as-you-go pricing

**Cons**:
- AWS-specific
- More configuration required

### Option 3: Docker + Kubernetes

**Best for**: Multi-cloud, containerized environments, maximum flexibility

**Pros**:
- Cloud-agnostic
- Highly scalable
- Portable across environments

**Cons**:
- More complex setup
- Requires container orchestration knowledge

### Option 4: Traditional IIS Hosting

**Best for**: On-premises, Windows Server environments

**Pros**:
- Full control
- No cloud costs
- Windows integration

**Cons**:
- Manual scaling
- Infrastructure management overhead

---

## ☁️ Azure Deployment

### Backend: Azure App Service

#### Step 1: Publish the Application

```bash
cd CloudCostOptimizer

# Publish for production
dotnet publish -c Release -o ./publish
```

#### Step 2: Create Azure Resources

```bash
# Login to Azure
az login

# Create resource group
az group create --name CloudOptimizerRG --location eastus

# Create App Service plan
az appservice plan create \
  --name CloudOptimizerPlan \
  --resource-group CloudOptimizerRG \
  --sku B1 \
  --is-linux

# Create Web App
az webapp create \
  --name cloudoptimizer-api \
  --resource-group CloudOptimizerRG \
  --plan CloudOptimizerPlan \
  --runtime "DOTNET|7.0"
```

#### Step 3: Deploy Application

```bash
# Deploy using Azure CLI
az webapp deployment source config-zip \
  --resource-group CloudOptimizerRG \
  --name cloudoptimizer-api \
  --src ./publish.zip

# Or use Visual Studio:
# Right-click project → Publish → Azure → Azure App Service
```

#### Step 4: Configure App Settings

```bash
# Set CORS origins
az webapp cors add \
  --resource-group CloudOptimizerRG \
  --name cloudoptimizer-api \
  --allowed-origins https://your-frontend-url.azurestaticapps.net

# Configure environment variables
az webapp config appsettings set \
  --resource-group CloudOptimizerRG \
  --name cloudoptimizer-api \
  --settings ASPNETCORE_ENVIRONMENT=Production
```

### Frontend: Azure Static Web Apps

#### Step 1: Prepare Frontend

Update `index.html` with production API URL:

```javascript
const API_BASE = 'https://cloudoptimizer-api.azurewebsites.net/api/cost';
```

#### Step 2: Deploy to Azure Static Web Apps

```bash
# Install Azure Static Web Apps CLI
npm install -g @azure/static-web-apps-cli

# Deploy
cd CloudCostOptimizerUI
swa deploy --app-location . --output-location . --api-location ""
```

Or use GitHub Actions for CI/CD:

```yaml
# .github/workflows/azure-static-web-apps.yml
name: Deploy to Azure Static Web Apps

on:
  push:
    branches: [main]

jobs:
  build_and_deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Build And Deploy
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: "upload"
          app_location: "/CloudCostOptimizerUI"
```

---

## 🌐 AWS Deployment

### Backend: AWS Elastic Beanstalk

#### Step 1: Install AWS CLI and EB CLI

```bash
# Install AWS CLI
pip install awscli

# Install EB CLI
pip install awsebcli

# Configure AWS credentials
aws configure
```

#### Step 2: Initialize Elastic Beanstalk

```bash
cd CloudCostOptimizer

# Initialize EB application
eb init -p "64bit Amazon Linux 2 v2.5.0 running .NET Core" cloudoptimizer-api --region us-east-1

# Create environment
eb create cloudoptimizer-prod --instance-type t3.small
```

#### Step 3: Deploy Application

```bash
# Deploy
eb deploy

# Open in browser
eb open
```

#### Step 4: Configure Environment

```bash
# Set environment variables
eb setenv ASPNETCORE_ENVIRONMENT=Production

# Configure CORS
# Add to appsettings.Production.json
```

### Frontend: S3 + CloudFront

#### Step 1: Create S3 Bucket

```bash
# Create bucket
aws s3 mb s3://cloudoptimizer-ui --region us-east-1

# Configure for static website hosting
aws s3 website s3://cloudoptimizer-ui \
  --index-document index.html \
  --error-document index.html
```

#### Step 2: Upload Files

```bash
cd CloudCostOptimizerUI

# Update API URL in index.html
# const API_BASE = 'http://your-eb-url.elasticbeanstalk.com/api/cost';

# Upload to S3
aws s3 sync . s3://cloudoptimizer-ui --acl public-read
```

#### Step 3: Create CloudFront Distribution

```bash
# Create distribution
aws cloudfront create-distribution \
  --origin-domain-name cloudoptimizer-ui.s3.amazonaws.com \
  --default-root-object index.html
```

---

## 🐳 Docker Deployment

### Step 1: Create Dockerfile for Backend

Create `CloudCostOptimizer/Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CloudCostOptimizer.csproj", "./"]
RUN dotnet restore "CloudCostOptimizer.csproj"
COPY . .
RUN dotnet build "CloudCostOptimizer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudCostOptimizer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudCostOptimizer.dll"]
```

### Step 2: Create Dockerfile for Frontend

Create `CloudCostOptimizerUI/Dockerfile`:

```dockerfile
FROM nginx:alpine
COPY . /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### Step 3: Create Docker Compose

Create `docker-compose.yml`:

```yaml
version: '3.8'

services:
  api:
    build:
      context: ./CloudCostOptimizer
      dockerfile: Dockerfile
    ports:
      - "5010:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:80
    networks:
      - cloudoptimizer

  ui:
    build:
      context: ./CloudCostOptimizerUI
      dockerfile: Dockerfile
    ports:
      - "8080:80"
    depends_on:
      - api
    networks:
      - cloudoptimizer

networks:
  cloudoptimizer:
    driver: bridge
```

### Step 4: Build and Run

```bash
# Build images
docker-compose build

# Run containers
docker-compose up -d

# View logs
docker-compose logs -f

# Stop containers
docker-compose down
```

### Step 5: Deploy to Kubernetes (Optional)

Create `k8s-deployment.yaml`:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: cloudoptimizer-api
spec:
  replicas: 3
  selector:
    matchLabels:
      app: cloudoptimizer-api
  template:
    metadata:
      labels:
        app: cloudoptimizer-api
    spec:
      containers:
      - name: api
        image: your-registry/cloudoptimizer-api:latest
        ports:
        - containerPort: 80
---
apiVersion: v1
kind: Service
metadata:
  name: cloudoptimizer-api-service
spec:
  selector:
    app: cloudoptimizer-api
  ports:
  - port: 80
    targetPort: 80
  type: LoadBalancer
```

Deploy:

```bash
kubectl apply -f k8s-deployment.yaml
```

---

## ⚙️ Configuration Management

### Environment-Specific Settings

Create `appsettings.Production.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": [
      "https://your-production-domain.com"
    ]
  }
}
```

### Environment Variables

Set these in your deployment environment:

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
```

### Secrets Management

For production, use:
- **Azure**: Azure Key Vault
- **AWS**: AWS Secrets Manager
- **Kubernetes**: Kubernetes Secrets

---

## 📊 Monitoring & Logging

### Application Insights (Azure)

```bash
# Install package
dotnet add package Microsoft.ApplicationInsights.AspNetCore

# Configure in Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### CloudWatch (AWS)

```bash
# Install package
dotnet add package AWS.Logger.AspNetCore

# Configure in appsettings.json
{
  "AWS.Logging": {
    "Region": "us-east-1",
    "LogGroup": "cloudoptimizer-api"
  }
}
```

### Health Checks

Add to `Program.cs`:

```csharp
builder.Services.AddHealthChecks();
app.MapHealthChecks("/health");
```

---

## 🔧 Troubleshooting

### Common Issues

#### 1. CORS Errors

**Problem**: Frontend can't connect to API

**Solution**:
```csharp
// In Program.cs, update CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("https://your-frontend-url.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
```

#### 2. API Not Responding

**Problem**: 502 Bad Gateway or timeout errors

**Solution**:
- Check application logs
- Verify port configuration
- Ensure health check endpoint is accessible
- Check firewall/security group rules

#### 3. Charts Not Loading

**Problem**: Dashboard shows but charts are empty

**Solution**:
- Check browser console for errors
- Verify API URL in frontend code
- Test API endpoints directly
- Check CORS configuration

#### 4. High Memory Usage

**Problem**: Application consuming too much memory

**Solution**:
- Implement caching with expiration
- Add pagination to large datasets
- Use streaming for large responses
- Monitor with Application Insights/CloudWatch

---

## 📝 Post-Deployment Checklist

- [ ] API is accessible and responding
- [ ] Frontend loads without errors
- [ ] All charts and metrics display correctly
- [ ] CORS is properly configured
- [ ] SSL/TLS certificates are valid
- [ ] Health check endpoint is working
- [ ] Logging is configured and working
- [ ] Monitoring/alerting is set up
- [ ] Backup strategy is in place
- [ ] Documentation is updated with URLs
- [ ] Performance testing completed
- [ ] Security scan completed

---

## 🔐 Security Best Practices

1. **Use HTTPS**: Always use SSL/TLS in production
2. **API Keys**: Implement API key authentication for production
3. **Rate Limiting**: Add rate limiting to prevent abuse
4. **Input Validation**: Validate all user inputs
5. **CORS**: Configure CORS to allow only trusted origins
6. **Secrets**: Never commit secrets to version control
7. **Updates**: Keep dependencies up to date
8. **Monitoring**: Set up security monitoring and alerts

---

## 📞 Support

For deployment issues:
1. Check application logs
2. Review this guide
3. Consult cloud provider documentation
4. Open an issue in the repository

---

**Last Updated**: 2026-05-21