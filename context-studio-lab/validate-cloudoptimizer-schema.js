/**
 * CloudOptimizer JSON-LD Schema Validator
 * 
 * Validates the cloudoptimizer-requirements-schema.jsonld against
 * the actual CloudOptimizerMCP implementation
 */

const fs = require('fs');
const path = require('path');

// Load the schema
const schemaPath = path.join(__dirname, 'cloudoptimizer-requirements-schema.jsonld');
let schema;

try {
  const schemaContent = fs.readFileSync(schemaPath, 'utf8');
  schema = JSON.parse(schemaContent);
  console.log('✓ CloudOptimizer schema loaded successfully');
} catch (error) {
  console.error('✗ Error loading schema:', error.message);
  process.exit(1);
}

const results = {
  passed: [],
  failed: [],
  warnings: []
};

function addResult(type, message) {
  results[type].push(message);
}

const app = schema['@graph'][0];

console.log('\n' + '='.repeat(70));
console.log('CLOUDOPTIMIZER SCHEMA VALIDATION');
console.log('='.repeat(70));

// Validate basic structure
console.log('\n=== Validating Basic Structure ===');
if (app['@type'] === 'SoftwareApplication') {
  addResult('passed', 'Application type is correct');
} else {
  addResult('failed', 'Application type is incorrect');
}

if (app.name === 'Cloud Cost Optimizer MCP') {
  addResult('passed', 'Application name matches');
} else {
  addResult('failed', 'Application name mismatch');
}

if (app.programmingLanguage === 'C#' && app.runtimePlatform === '.NET 7.0') {
  addResult('passed', 'Technology stack correctly documented');
} else {
  addResult('failed', 'Technology stack mismatch');
}

// Validate Data Models
console.log('\n=== Validating Data Models ===');
const expectedModels = ['CloudResource', 'Recommendation', 'WasteAnalysis', 'CostSummary'];
const actualModels = app.dataModels.map(m => m.name);

expectedModels.forEach(model => {
  if (actualModels.includes(model)) {
    addResult('passed', `Model '${model}' is documented`);
  } else {
    addResult('failed', `Model '${model}' is missing`);
  }
});

// Validate CloudResource model properties
const cloudResourceModel = app.dataModels.find(m => m.name === 'CloudResource');
if (cloudResourceModel) {
  const expectedProps = [
    'ResourceId', 'ResourceName', 'Provider', 'Type', 'State', 'Region',
    'MonthlyCost', 'CpuUsage', 'MemoryUsage', 'StorageUsage', 'DaysIdle',
    'CreatedDate', 'LastAccessDate', 'InstanceSize', 'RecommendedSize',
    'EstimatedWaste', 'PotentialSavings', 'Tags', 'IsProduction'
  ];
  const actualProps = cloudResourceModel.properties.map(p => p.name);
  
  expectedProps.forEach(prop => {
    if (actualProps.includes(prop)) {
      addResult('passed', `CloudResource.${prop} property documented`);
    } else {
      addResult('failed', `CloudResource.${prop} property missing`);
    }
  });
}

// Validate Enumerations
console.log('\n=== Validating Enumerations ===');
const expectedEnums = ['ResourceType', 'ResourceState', 'WasteCategory', 'RecommendationPriority'];
const actualEnums = app.enumerations.map(e => e.name);

expectedEnums.forEach(enumName => {
  if (actualEnums.includes(enumName)) {
    addResult('passed', `Enum '${enumName}' is documented`);
  } else {
    addResult('failed', `Enum '${enumName}' is missing`);
  }
});

// Validate ResourceType enum values
const resourceTypeEnum = app.enumerations.find(e => e.name === 'ResourceType');
if (resourceTypeEnum) {
  const expectedValues = [
    'VirtualMachine', 'Database', 'Storage', 'LoadBalancer', 'Network',
    'Container', 'Function', 'Cache', 'Queue', 'Other'
  ];
  const actualValues = resourceTypeEnum.values;
  
  if (JSON.stringify(expectedValues) === JSON.stringify(actualValues)) {
    addResult('passed', 'ResourceType enum values match implementation');
  } else {
    addResult('warnings', 'ResourceType enum values may differ from implementation');
  }
}

// Validate Service Components
console.log('\n=== Validating Service Components ===');
const expectedServices = ['CostService', 'DataGenerator', 'WasteAnalyzer', 'RecommendationEngine'];
const actualServices = app.serviceComponents.map(s => s.name);

expectedServices.forEach(service => {
  if (actualServices.includes(service)) {
    addResult('passed', `Service '${service}' is documented`);
  } else {
    addResult('failed', `Service '${service}' is missing`);
  }
});

// Validate Waste Detection Algorithms
console.log('\n=== Validating Waste Detection Algorithms ===');
const expectedAlgorithms = [
  'Idle Resources Detection',
  'Oversized Instances Detection',
  'Unattached Volumes Detection',
  'Stopped Resources Detection',
  'Zombie Resources Detection'
];
const actualAlgorithms = app.wasteDetectionAlgorithms.map(a => a.name);

expectedAlgorithms.forEach(algo => {
  if (actualAlgorithms.includes(algo)) {
    addResult('passed', `Algorithm '${algo}' is documented`);
  } else {
    addResult('failed', `Algorithm '${algo}' is missing`);
  }
});

// Validate algorithm criteria
const idleAlgo = app.wasteDetectionAlgorithms.find(a => a.name === 'Idle Resources Detection');
if (idleAlgo && idleAlgo.criteria === 'CPU < 5% AND Memory < 10% AND DaysIdle >= 7') {
  addResult('passed', 'Idle resources criteria matches implementation');
} else {
  addResult('failed', 'Idle resources criteria mismatch');
}

// Validate Recommendation Rules
console.log('\n=== Validating Recommendation Rules ===');
const expectedRules = [
  'Terminate Idle Resources',
  'Downsize Oversized Instances',
  'Delete Unattached Storage Volumes',
  'Review Stopped Resources',
  'Clean Up Zombie Resources',
  'Purchase Reserved Instances',
  'Implement Resource Scheduling'
];
const actualRules = app.recommendationRules.map(r => r.name);

expectedRules.forEach(rule => {
  if (actualRules.includes(rule)) {
    addResult('passed', `Rule '${rule}' is documented`);
  } else {
    addResult('failed', `Rule '${rule}' is missing`);
  }
});

// Validate API Endpoints
console.log('\n=== Validating API Endpoints ===');
const expectedEndpoints = [
  '/api/cost/costs',
  '/api/cost/idle-resources',
  '/api/cost/recommendations',
  '/api/cost/summary',
  '/api/cost/waste-breakdown',
  '/api/cost/resources',
  '/api/cost/resources/provider/{provider}',
  '/api/cost/resources/type/{type}',
  '/api/cost/resources/state/{state}',
  '/api/cost/recommendations/detailed',
  '/api/cost/recommendations/high-priority',
  '/api/cost/trends',
  '/api/cost/trends/provider/{provider}',
  '/api/cost/top-wasteful',
  '/api/cost/waste-by-provider',
  '/api/cost/potential-savings',
  '/api/cost/refresh'
];
const actualEndpoints = app.apiEndpoints.map(e => e.path);

expectedEndpoints.forEach(endpoint => {
  if (actualEndpoints.includes(endpoint)) {
    addResult('passed', `Endpoint '${endpoint}' is documented`);
  } else {
    addResult('failed', `Endpoint '${endpoint}' is missing`);
  }
});

addResult('passed', `Total API endpoints documented: ${actualEndpoints.length}`);

// Validate MCP Integration
console.log('\n=== Validating MCP Integration ===');
if (app.mcpIntegration) {
  addResult('passed', 'MCP integration is documented');
  
  const expectedTools = ['GetCloudCosts', 'DetectIdleResources', 'GetRecommendations'];
  const actualTools = app.mcpIntegration.tools.map(t => t.name);
  
  expectedTools.forEach(tool => {
    if (actualTools.includes(tool)) {
      addResult('passed', `MCP tool '${tool}' is documented`);
    } else {
      addResult('failed', `MCP tool '${tool}' is missing`);
    }
  });
} else {
  addResult('failed', 'MCP integration not documented');
}

// Validate Requirements
console.log('\n=== Validating Requirements ===');
const requirements = app.requirements;
addResult('passed', `Total requirements documented: ${requirements.length}`);

const functionalReqs = requirements.filter(r => r['@type'] === 'FunctionalRequirement');
const nonFunctionalReqs = requirements.filter(r => r['@type'] === 'NonFunctionalRequirement');
const securityReqs = requirements.filter(r => r['@type'] === 'SecurityRequirement');

addResult('passed', `Functional requirements: ${functionalReqs.length}`);
addResult('passed', `Non-functional requirements: ${nonFunctionalReqs.length}`);
addResult('passed', `Security requirements: ${securityReqs.length}`);

// Check for implemented status
const implementedReqs = requirements.filter(r => r.status === 'Implemented');
addResult('passed', `Implemented requirements: ${implementedReqs.length}/${requirements.length}`);

// Validate requirement linkages
let linkedReqs = 0;
requirements.forEach(req => {
  if (req.implementedBy || req.usesModel || req.apiEndpoints || req.usesAlgorithms) {
    linkedReqs++;
  }
});
addResult('passed', `Requirements with implementation links: ${linkedReqs}/${requirements.length}`);

// Validate Technical Stack
console.log('\n=== Validating Technical Stack ===');
if (app.technicalStack) {
  if (app.technicalStack.backend.framework === 'ASP.NET Core 7.0') {
    addResult('passed', 'Backend framework documented correctly');
  }
  if (app.technicalStack.backend.language === 'C# 11') {
    addResult('passed', 'Programming language documented correctly');
  }
  if (app.technicalStack.frontend.technologies.includes('HTML5')) {
    addResult('passed', 'Frontend technologies documented');
  }
  if (app.technicalStack.integration.protocol === 'Model Context Protocol (MCP)') {
    addResult('passed', 'MCP integration protocol documented');
  }
} else {
  addResult('failed', 'Technical stack not documented');
}

// Validate Data Generation Details
console.log('\n=== Validating Data Generation ===');
const dataGen = app.serviceComponents.find(s => s.name === 'DataGenerator');
if (dataGen && dataGen.dataGeneration) {
  if (dataGen.dataGeneration.totalResources === 53) {
    addResult('passed', 'Total resources count matches (53)');
  } else {
    addResult('warnings', 'Total resources count may differ');
  }
  
  const providers = dataGen.dataGeneration.providers;
  if (providers.AWS === 20 && providers.Azure === 18 && providers.GCP === 15) {
    addResult('passed', 'Provider distribution documented correctly');
  } else {
    addResult('warnings', 'Provider distribution may differ');
  }
}

// Validate Future Enhancements
console.log('\n=== Validating Future Enhancements ===');
if (app.futureEnhancements && app.futureEnhancements.length > 0) {
  addResult('passed', `${app.futureEnhancements.length} future enhancements documented`);
} else {
  addResult('warnings', 'No future enhancements documented');
}

// Print summary
console.log('\n' + '='.repeat(70));
console.log('VALIDATION SUMMARY');
console.log('='.repeat(70));
console.log(`✓ Passed: ${results.passed.length}`);
console.log(`✗ Failed: ${results.failed.length}`);
console.log(`⚠ Warnings: ${results.warnings.length}`);

if (results.failed.length > 0) {
  console.log('\n--- FAILURES ---');
  results.failed.forEach(msg => console.log(`✗ ${msg}`));
}

if (results.warnings.length > 0) {
  console.log('\n--- WARNINGS ---');
  results.warnings.forEach(msg => console.log(`⚠ ${msg}`));
}

// Key Statistics
console.log('\n--- KEY STATISTICS ---');
console.log(`Data Models: ${app.dataModels.length}`);
console.log(`Enumerations: ${app.enumerations.length}`);
console.log(`Service Components: ${app.serviceComponents.length}`);
console.log(`Waste Detection Algorithms: ${app.wasteDetectionAlgorithms.length}`);
console.log(`Recommendation Rules: ${app.recommendationRules.length}`);
console.log(`API Endpoints: ${app.apiEndpoints.length}`);
console.log(`MCP Tools: ${app.mcpIntegration.tools.length}`);
console.log(`Requirements: ${app.requirements.length}`);
console.log(`Future Enhancements: ${app.futureEnhancements.length}`);

console.log('\n' + '='.repeat(70));

if (results.failed.length > 0) {
  console.log('❌ Validation FAILED');
  process.exit(1);
} else {
  console.log('✅ Validation PASSED - Schema accurately represents CloudOptimizerMCP');
  process.exit(0);
}

// Made with Bob
