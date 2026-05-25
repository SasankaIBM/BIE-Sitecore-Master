# Cloud Cost Optimizer - Functional Requirements

## Overview
This document details the functional requirements for the Cloud Cost Optimizer MCP system. These requirements define the specific behaviors and functions that the system must provide to detect cloud waste, analyze costs, and generate optimization recommendations.

---

## Table of Contents
1. [Core Entities](#core-entities)
2. [Functional Operations](#functional-operations)
3. [State Management](#state-management)
4. [Requirement Details](#requirement-details)

---

## Core Entities

### 1. CloudResource Entity
**Identity Key**: `resourceId: UUID`  
**Human Reference**: `resourceName`

#### Description
A cloud infrastructure resource across AWS, Azure, or GCP that consumes cost and can be optimized.

#### Attributes
- **resourceId** (string): Unique identifier for the resource
- **resourceName** (string): Human-readable name of the resource
- **provider** (AWS|Azure|GCP): Cloud service provider
- **resourceType** (VirtualMachine|Database|Storage|LoadBalancer|Network|Container|Function|Cache|Queue|Other): Type of cloud resource
- **region** (string): Geographic region where resource is deployed
- **monthlyCost** (decimal): Current monthly cost of the resource
- **cpuUtilization** (decimal): CPU usage percentage (0-100)
- **memoryUtilization** (decimal): Memory usage percentage (0-100)
- **storageUtilization** (decimal): Storage usage percentage (0-100)
- **daysIdle** (integer): Number of days resource has been idle
- **lastAccessDate** (datetime): Last time resource was accessed
- **state** (Running|Stopped|Terminated): Current operational state
- **tags** (object): Metadata tags associated with the resource

#### Invariants
- resourceId must be unique
- provider must be one of AWS, Azure, or GCP
- resourceType must be one of defined types
- monthlyCost must be non-negative
- utilization percentages must be between 0 and 100

#### States
- **ResourceActive**: Resource is running and being actively used
- **ResourceIdle**: Resource is running but has low utilization (CPU < 5%, Memory < 10%)
- **ResourceOversized**: Resource is running but oversized for its workload (CPU < 20%, Memory < 30%)
- **ResourceStopped**: Resource is stopped but still incurring costs
- **ResourceZombie**: Resource has not been accessed for over 30 days
- **ResourceTerminated**: Resource has been terminated and is no longer incurring costs

#### Events Emitted
- ResourceStateChanged
- WasteDetected
- CostThresholdExceeded

---

### 2. WasteAnalysis Entity
**Identity Key**: `analysisId: UUID`  
**Human Reference**: `wasteCategory`

#### Description
Analysis result identifying waste in cloud resources with potential savings calculations.

#### Attributes
- **analysisId** (string): Unique identifier for the analysis
- **resourceId** (string): Reference to the analyzed CloudResource
- **wasteCategory** (IdleResources|OversizedInstances|UnattachedVolumes|StoppedResources|ZombieResources|RedundantBackups): Category of detected waste
- **detectedDate** (datetime): When the waste was detected
- **monthlySavings** (decimal): Potential monthly savings
- **annualSavings** (decimal): Potential annual savings
- **savingsPercentage** (decimal): Percentage of cost that can be saved (0-100)
- **confidence** (High|Medium|Low): Confidence level of the analysis
- **detectionCriteria** (string): Criteria used to detect the waste
- **algorithmUsed** (string): Algorithm that detected the waste

#### Invariants
- analysisId must be unique
- resourceId must reference existing CloudResource
- wasteCategory must be one of defined categories
- savings must be non-negative
- confidence must be High, Medium, or Low
- savingsPercentage must be between 0 and 100

#### States
- **WasteDetected**: Waste has been detected by an algorithm
- **WasteConfirmed**: Waste has been confirmed and recommendation generated
- **WasteAddressed**: Waste has been addressed through recommendation implementation
- **WasteFalsePositive**: Detected waste was determined to be a false positive

#### Events Emitted
- WasteConfirmed
- RecommendationGenerated

---

### 3. Recommendation Entity
**Identity Key**: `recommendationId: UUID`  
**Human Reference**: `title`

#### Description
Actionable optimization recommendation with priority, effort, and ROI calculations.

#### Attributes
- **recommendationId** (string): Unique identifier for the recommendation
- **resourceId** (string): Reference to the CloudResource
- **title** (string): Brief title of the recommendation
- **description** (string): Detailed description of the recommendation
- **actionType** (Terminate|Downsize|Delete|Review|Schedule|Purchase|Migrate|Configure): Type of action to take
- **priority** (High|Medium|Low): Priority level based on savings and impact
- **implementationEffort** (integer): Effort required to implement (1-5 scale)
- **monthlySavings** (decimal): Expected monthly savings
- **annualSavings** (decimal): Expected annual savings
- **roi** (decimal): Return on investment calculation
- **status** (Pending|InProgress|Implemented|Rejected|Deferred): Current status
- **createdDate** (datetime): When recommendation was created
- **implementedDate** (datetime|null): When recommendation was implemented
- **estimatedImplementationTime** (string): Estimated time to implement

#### Invariants
- recommendationId must be unique
- resourceId must reference existing CloudResource
- actionType must be one of defined types
- priority must be High, Medium, or Low
- implementationEffort must be between 1 and 5
- savings must be non-negative
- status must be one of defined values

#### States
- **RecommendationPending**: Recommendation has been generated and is awaiting review
- **RecommendationInProgress**: Recommendation is being implemented
- **RecommendationImplemented**: Recommendation has been successfully implemented
- **RecommendationRejected**: Recommendation has been rejected and will not be implemented
- **RecommendationDeferred**: Recommendation has been deferred for future consideration

#### Events Emitted
- RecommendationImplemented
- SavingsRealized

---

### 4. WasteDetectionAlgorithm Entity
**Identity Key**: `algorithmId: UUID`  
**Human Reference**: `algorithmName`

#### Description
Algorithm that analyzes resources to detect specific waste patterns.

#### Attributes
- **algorithmId** (string): Unique identifier for the algorithm
- **algorithmName** (string): Name of the algorithm
- **wasteCategory** (string): Category of waste this algorithm detects
- **detectionCriteria** (string): Criteria used for detection
- **savingsCalculationFormula** (string): Formula to calculate potential savings
- **confidenceThreshold** (decimal): Minimum confidence threshold (0-1)
- **implementedIn** (string): Service component where algorithm is implemented
- **version** (string): Version of the algorithm

#### Invariants
- algorithmId must be unique
- algorithmName must be unique
- detectionCriteria must be defined
- savingsCalculationFormula must be valid
- confidenceThreshold must be between 0 and 1

#### Waste Detection Algorithms

##### 1. Idle Resources Detection
- **Criteria**: CPU < 5% AND Memory < 10% AND DaysIdle >= 7
- **Savings Rate**: 85% of resource cost
- **Action**: Terminate or stop the resource

##### 2. Oversized Instances Detection
- **Criteria**: CPU < 20% AND Memory < 30% AND Type = VM
- **Savings Rate**: 35% of resource cost
- **Action**: Downsize to smaller instance type

##### 3. Unattached Volumes Detection
- **Criteria**: Type = Storage AND DaysIdle > 30
- **Savings Rate**: 100% of storage cost
- **Action**: Delete unattached volumes

##### 4. Stopped Resources Detection
- **Criteria**: State = Stopped
- **Savings Rate**: 25% of resource cost (storage costs remain)
- **Action**: Review and terminate if not needed

##### 5. Zombie Resources Detection
- **Criteria**: LastAccessDate > 30 days ago
- **Savings Rate**: 90% of resource cost
- **Action**: Clean up unused resources

---

### 5. RecommendationRule Entity
**Identity Key**: `ruleId: UUID`  
**Human Reference**: `ruleName`

#### Description
Business rule that generates recommendations based on waste analysis.

#### Attributes
- **ruleId** (string): Unique identifier for the rule
- **ruleName** (string): Name of the rule
- **actionType** (string): Type of action the rule recommends
- **priorityLogic** (string): Logic to determine recommendation priority
- **effortEstimation** (integer): Estimated effort to implement (1-5)
- **savingsCalculation** (string): Formula to calculate savings
- **applicableWasteCategories** (array): Waste categories this rule applies to
- **implementedIn** (string): Service component where rule is implemented

#### Invariants
- ruleId must be unique
- ruleName must be unique
- actionType must be defined
- effortEstimation must be between 1 and 5
- applicableWasteCategories must not be empty

---

### 6. CostReport Entity
**Identity Key**: `reportId: UUID`  
**Human Reference**: `reportName`

#### Description
Aggregated cost and savings report for a time period.

#### Attributes
- **reportId** (string): Unique identifier for the report
- **reportName** (string): Name of the report
- **reportType** (Monthly|Quarterly|Annual|Custom): Type of report
- **startDate** (datetime): Report period start date
- **endDate** (datetime): Report period end date
- **totalCost** (decimal): Total cost for the period
- **totalWaste** (decimal): Total waste identified
- **potentialSavings** (decimal): Total potential savings
- **realizedSavings** (decimal): Actual savings realized
- **wastePercentage** (decimal): Percentage of total cost that is waste
- **resourceCount** (integer): Number of resources analyzed
- **providerBreakdown** (object): Cost breakdown by cloud provider

#### Invariants
- reportId must be unique
- endDate must be after startDate
- all cost values must be non-negative
- wastePercentage must be between 0 and 100

---

## Functional Operations

### 1. AnalyzeResource Operation
**From**: CloudResource  
**To**: WasteAnalysis

#### Description
Analyzes a cloud resource to detect waste patterns using configured algorithms.

#### Preconditions
- CloudResource must exist and be in Active or Idle state
- Resource must have utilization metrics
- WasteDetectionAlgorithm must be configured

#### Postconditions
- WasteAnalysis is created if waste is detected
- Resource state may transition to Idle, Oversized, or Zombie
- WasteDetected event is emitted if applicable

#### Process Flow
1. Retrieve resource utilization metrics
2. Apply waste detection algorithms
3. Calculate potential savings
4. Determine confidence level
5. Create WasteAnalysis record
6. Update resource state
7. Emit WasteDetected event

---

### 2. GenerateRecommendation Operation
**From**: WasteAnalysis  
**To**: Recommendation

#### Description
Generates an actionable recommendation based on waste analysis using recommendation rules.

#### Preconditions
- WasteAnalysis must be in Detected or Confirmed state
- RecommendationRule must be applicable to waste category
- Savings calculation must be positive

#### Postconditions
- Recommendation is created with priority and effort
- WasteAnalysis transitions to Confirmed state
- RecommendationGenerated event is emitted

#### Process Flow
1. Retrieve waste analysis details
2. Apply recommendation rules
3. Calculate ROI and priority
4. Estimate implementation effort
5. Create Recommendation record
6. Update waste analysis state
7. Emit RecommendationGenerated event

---

### 3. ImplementRecommendation Operation
**From**: Recommendation  
**To**: CloudResource

#### Description
Implements a recommendation and updates resource state accordingly.

#### Preconditions
- Recommendation must be in Pending or InProgress state
- CloudResource must exist and be modifiable
- User authorization must be verified

#### Postconditions
- Recommendation transitions to Implemented state
- CloudResource state is updated based on action type
- SavingsRealized event is emitted
- WasteAnalysis transitions to Addressed state

#### Process Flow
1. Verify user authorization
2. Validate recommendation is implementable
3. Execute action (terminate, downsize, delete, etc.)
4. Update resource state
5. Update recommendation status
6. Calculate realized savings
7. Emit SavingsRealized event

---

### 4. MonitorResource Operation
**From**: CloudResource  
**To**: CloudResource

#### Description
Continuously monitors resource utilization and cost metrics.

#### Preconditions
- CloudResource must be in Active state
- Monitoring metrics must be available

#### Postconditions
- Resource utilization metrics are updated
- CostThresholdExceeded event may be emitted
- Resource state may transition based on utilization

#### Process Flow
1. Collect current utilization metrics
2. Update resource attributes
3. Check for threshold violations
4. Determine if state transition is needed
5. Emit events as appropriate

---

### 5. GenerateCostReport Operation
**From**: CloudResource  
**To**: CostReport

#### Description
Generates aggregated cost and savings report for a specified time period.

#### Preconditions
- Time period must be specified
- CloudResources must exist for the period
- WasteAnalysis data must be available

#### Postconditions
- CostReport is created with aggregated metrics
- Provider breakdown is calculated
- Waste percentage is computed

#### Process Flow
1. Define report parameters (period, filters)
2. Aggregate resource costs
3. Aggregate waste analysis data
4. Calculate savings metrics
5. Generate provider breakdown
6. Create CostReport record

---

## State Management

### Resource State Transitions

```
ResourceActive → ResourceIdle (when utilization drops below thresholds)
ResourceActive → ResourceOversized (when consistently underutilized)
ResourceActive → ResourceStopped (when manually stopped)
ResourceIdle → ResourceZombie (when idle for > 30 days)
ResourceIdle → ResourceActive (when utilization increases)
ResourceStopped → ResourceTerminated (when deleted)
ResourceZombie → ResourceTerminated (when cleaned up)
Any State → ResourceTerminated (terminal state)
```

### Waste Analysis State Transitions

```
WasteDetected → WasteConfirmed (when recommendation is generated)
WasteDetected → WasteFalsePositive (when analysis is incorrect)
WasteConfirmed → WasteAddressed (when recommendation is implemented)
```

### Recommendation State Transitions

```
RecommendationPending → RecommendationInProgress (when implementation starts)
RecommendationPending → RecommendationRejected (when rejected by user)
RecommendationPending → RecommendationDeferred (when postponed)
RecommendationInProgress → RecommendationImplemented (when completed)
RecommendationInProgress → RecommendationRejected (when cancelled)
```

---

## Requirement Details

### FR-001: Multi-Cloud Resource Monitoring
**Priority**: High  
**Status**: Must Implement

#### Description
The system shall monitor cloud resources across AWS, Azure, and GCP, tracking utilization metrics and costs in real-time.

#### Acceptance Criteria
- Support AWS, Azure, and GCP providers
- Track 10 different resource types
- Monitor 50+ resources simultaneously
- Capture utilization metrics (CPU, Memory, Storage)
- Update metrics at configurable intervals

#### Implementation
- **Entity**: CloudResource
- **Operation**: MonitorResource
- **Service**: CostService, DataGenerator

---

### FR-002: Intelligent Waste Detection
**Priority**: High  
**Status**: Must Implement

#### Description
The system shall detect 6 categories of cloud waste using specific algorithms with configurable thresholds.

#### Acceptance Criteria
- Detect idle resources (CPU < 5%, Memory < 10%, Days >= 7)
- Detect oversized instances (CPU < 20%, Memory < 30%)
- Detect unattached volumes (Storage idle > 30 days)
- Detect stopped resources with ongoing costs
- Detect zombie resources (not accessed > 30 days)
- Detect redundant backups
- Calculate potential savings for each waste category

#### Implementation
- **Entity**: WasteAnalysis, WasteDetectionAlgorithm
- **Operation**: AnalyzeResource
- **Service**: WasteAnalyzer

---

### FR-003: Smart Recommendation Engine
**Priority**: High  
**Status**: Must Implement

#### Description
The system shall generate prioritized, actionable recommendations with ROI calculations based on detected waste.

#### Acceptance Criteria
- Generate recommendations for each waste category
- Calculate priority based on savings potential
- Estimate implementation effort (1-5 scale)
- Calculate ROI for each recommendation
- Provide detailed action steps
- Track recommendation status

#### Implementation
- **Entity**: Recommendation, RecommendationRule
- **Operation**: GenerateRecommendation
- **Service**: RecommendationEngine

---

### FR-004: Cost Reporting and Analytics
**Priority**: High  
**Status**: Must Implement

#### Description
The system shall generate comprehensive cost reports with waste analysis and savings projections.

#### Acceptance Criteria
- Generate monthly, quarterly, and annual reports
- Provide cost breakdown by provider
- Show waste percentage and categories
- Display potential vs. realized savings
- Support custom date ranges
- Export reports in multiple formats

#### Implementation
- **Entity**: CostReport
- **Operation**: GenerateCostReport
- **Service**: CostService

---

### FR-005: Recommendation Implementation Tracking
**Priority**: Medium  
**Status**: Should Implement

#### Description
The system shall track the implementation status of recommendations and measure realized savings.

#### Acceptance Criteria
- Track recommendation lifecycle (Pending → InProgress → Implemented)
- Record implementation date and user
- Calculate realized savings
- Compare projected vs. actual savings
- Provide implementation history

#### Implementation
- **Entity**: Recommendation
- **Operation**: ImplementRecommendation
- **Service**: RecommendationEngine

---

### FR-006: Resource Tagging and Categorization
**Priority**: Medium  
**Status**: Should Implement

#### Description
The system shall support resource tagging for better organization and filtering.

#### Acceptance Criteria
- Support custom tags on resources
- Filter resources by tags
- Group resources by tags in reports
- Tag-based cost allocation
- Tag inheritance for related resources

#### Implementation
- **Entity**: CloudResource (tags attribute)
- **Service**: CostService

---

### FR-007: Alert and Notification System
**Priority**: Medium  
**Status**: Should Implement

#### Description
The system shall send alerts when waste is detected or cost thresholds are exceeded.

#### Acceptance Criteria
- Configurable alert thresholds
- Multiple notification channels (email, webhook)
- Alert prioritization
- Alert history and tracking
- Customizable alert templates

#### Implementation
- **Events**: WasteDetected, CostThresholdExceeded
- **Service**: New AlertService (to be implemented)

---

### FR-008: Historical Trend Analysis
**Priority**: Low  
**Status**: Could Implement

#### Description
The system shall maintain historical data and provide trend analysis for costs and waste patterns.

#### Acceptance Criteria
- Store historical metrics
- Display cost trends over time
- Show waste pattern evolution
- Predict future costs based on trends
- Compare periods (month-over-month, year-over-year)

#### Implementation
- **Entity**: CostReport (with historical data)
- **Service**: CostService (enhanced with analytics)

---

## Integration Points

### API Endpoints
The functional requirements are exposed through RESTful API endpoints:

- `GET /api/cost/resources` - List all cloud resources
- `GET /api/cost/resources/{id}` - Get specific resource details
- `GET /api/cost/waste-analysis` - Get waste analysis results
- `GET /api/cost/recommendations` - Get all recommendations
- `POST /api/cost/recommendations/{id}/implement` - Implement a recommendation
- `GET /api/cost/reports` - Get cost reports
- `POST /api/cost/reports/generate` - Generate new report

### MCP Tools
The system provides MCP tools for AI assistant integration:

- `GetCloudCosts` - Retrieve current cloud costs
- `DetectIdleResources` - Detect idle resources
- `GetRecommendations` - Get optimization recommendations

---

## Success Metrics

### Key Performance Indicators
- **Waste Detection Accuracy**: > 95% true positive rate
- **Recommendation Acceptance Rate**: > 70% of high-priority recommendations
- **Savings Realization**: > 80% of projected savings achieved
- **Response Time**: < 2 seconds for resource analysis
- **System Uptime**: > 99.9% availability

### Business Outcomes
- Reduce cloud costs by 20-40%
- Identify waste within 24 hours of occurrence
- Provide actionable recommendations within minutes
- Track and measure savings realization
- Improve resource utilization efficiency

---

## Glossary

- **Waste**: Cloud resources that are underutilized or unnecessary, resulting in avoidable costs
- **Idle Resource**: A resource with very low utilization (CPU < 5%, Memory < 10%)
- **Oversized Instance**: A resource that is larger than needed for its workload
- **Zombie Resource**: A resource that hasn't been accessed in over 30 days
- **ROI**: Return on Investment - the ratio of savings to implementation effort
- **Savings Realization**: The actual cost reduction achieved after implementing recommendations

---

*Document Version: 1.0*  
*Last Updated: 2026-05-22*  
*Based on: cloudoptimizer-requirements-schema.jsonld*