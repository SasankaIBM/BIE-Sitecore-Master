# Cloud Cost Optimizer - Non-Functional Requirements

## Overview
This document details the non-functional requirements (NFRs) for the Cloud Cost Optimizer MCP system. These requirements define the quality attributes, constraints, and system-wide characteristics that ensure the system is performant, secure, reliable, maintainable, and usable.

---

## Table of Contents
1. [Performance Requirements](#performance-requirements)
2. [Security Requirements](#security-requirements)
3. [Reliability Requirements](#reliability-requirements)
4. [Scalability Requirements](#scalability-requirements)
5. [Maintainability Requirements](#maintainability-requirements)
6. [Usability Requirements](#usability-requirements)
7. [Compatibility Requirements](#compatibility-requirements)
8. [Compliance Requirements](#compliance-requirements)

---

## Performance Requirements

### NFR-PERF-001: Response Time
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Performance

#### Description
The system shall provide fast response times for all user interactions and API calls.

#### Acceptance Criteria
- API endpoint response time < 2 seconds for 95% of requests
- Resource analysis completion < 5 seconds for single resource
- Bulk analysis (50+ resources) completion < 30 seconds
- Dashboard load time < 3 seconds
- Report generation < 10 seconds for monthly reports

#### Measurement Criteria
- Average response time measured via application performance monitoring (APM)
- 95th percentile response time < 2 seconds
- 99th percentile response time < 5 seconds

#### Implementation Details
- Use asynchronous processing for long-running operations
- Implement caching for frequently accessed data
- Optimize database queries with proper indexing
- Use connection pooling for database connections

---

### NFR-PERF-002: Throughput
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Performance

#### Description
The system shall handle concurrent requests efficiently without degradation.

#### Acceptance Criteria
- Support 100 concurrent users
- Process 1000 API requests per minute
- Analyze 500 resources per minute
- Generate 50 recommendations per minute

#### Measurement Criteria
- Requests per second (RPS) under load
- Concurrent user capacity
- Resource analysis throughput

#### Implementation Details
- Use thread pooling for concurrent operations
- Implement request queuing for high load scenarios
- Use load balancing for distributed deployments

---

### NFR-PERF-003: Resource Utilization
**Priority**: Medium  
**Status**: Should Implement  
**Quality Attribute**: Performance

#### Description
The system shall efficiently use computing resources (CPU, memory, storage).

#### Acceptance Criteria
- CPU utilization < 70% under normal load
- Memory usage < 2GB for typical workload
- Database storage growth < 1GB per month
- Efficient garbage collection (< 5% CPU time)

#### Measurement Criteria
- Monitor CPU, memory, and disk usage
- Track resource consumption trends
- Measure garbage collection frequency and duration

#### Implementation Details
- Implement efficient data structures
- Use streaming for large data processing
- Implement data retention policies
- Optimize memory allocation patterns

---

### NFR-PERF-004: Data Freshness
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Performance

#### Description
The system shall provide up-to-date resource and cost information.

#### Acceptance Criteria
- Resource metrics updated every 5 minutes
- Cost data refreshed every 15 minutes
- Waste analysis runs every 30 minutes
- Real-time updates for critical events

#### Measurement Criteria
- Data staleness (time since last update)
- Update frequency compliance
- Event processing latency

#### Implementation Details
- Implement scheduled background jobs
- Use event-driven architecture for real-time updates
- Cache with appropriate TTL values

---

## Security Requirements

### NFR-SEC-001: Authentication
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Security

#### Description
The system shall authenticate all users and API clients before granting access.

#### Acceptance Criteria
- Support OAuth 2.0 / OpenID Connect authentication
- Support API key authentication for MCP tools
- Enforce strong password policies (min 12 chars, complexity)
- Support multi-factor authentication (MFA)
- Session timeout after 30 minutes of inactivity

#### Measurement Criteria
- Authentication success/failure rate
- MFA adoption rate
- Session security compliance

#### Implementation Details
- Integrate with identity providers (Azure AD, Okta, etc.)
- Implement JWT token-based authentication
- Store passwords using bcrypt with salt
- Implement rate limiting on authentication endpoints

---

### NFR-SEC-002: Authorization
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Security

#### Description
The system shall enforce role-based access control (RBAC) for all operations.

#### Acceptance Criteria
- Define roles: Admin, Analyst, Viewer, API Client
- Admins can modify system configuration
- Analysts can view data and implement recommendations
- Viewers can only view reports and dashboards
- API clients have limited scope based on API key

#### Measurement Criteria
- Authorization check coverage (100% of protected endpoints)
- Access violation attempts logged
- Role assignment audit trail

#### Implementation Details
- Implement RBAC middleware
- Use claims-based authorization
- Audit all authorization decisions
- Implement principle of least privilege

---

### NFR-SEC-003: Data Encryption
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Security

#### Description
The system shall encrypt sensitive data at rest and in transit.

#### Acceptance Criteria
- All API communication over HTTPS/TLS 1.3
- Database encryption at rest using AES-256
- Encrypt sensitive configuration values
- Secure credential storage (no plaintext passwords)
- Encrypt backup files

#### Measurement Criteria
- TLS version compliance
- Encryption coverage for sensitive data
- Certificate validity monitoring

#### Implementation Details
- Use ASP.NET Core Data Protection API
- Configure HTTPS with valid SSL certificates
- Use Azure Key Vault or similar for secrets management
- Implement database transparent data encryption (TDE)

---

### NFR-SEC-004: Audit Logging
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Security

#### Description
The system shall log all security-relevant events for audit and compliance.

#### Acceptance Criteria
- Log all authentication attempts (success/failure)
- Log all authorization decisions
- Log all data modifications (who, what, when)
- Log all recommendation implementations
- Retain audit logs for 1 year minimum
- Protect audit logs from tampering

#### Measurement Criteria
- Audit log completeness
- Log retention compliance
- Log integrity verification

#### Implementation Details
- Use structured logging (JSON format)
- Implement centralized log aggregation
- Use write-once storage for audit logs
- Include correlation IDs for request tracing

---

### NFR-SEC-005: Input Validation
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Security

#### Description
The system shall validate and sanitize all user inputs to prevent injection attacks.

#### Acceptance Criteria
- Validate all API request parameters
- Sanitize user-provided strings
- Prevent SQL injection attacks
- Prevent cross-site scripting (XSS)
- Implement request size limits
- Validate file uploads (if applicable)

#### Measurement Criteria
- Input validation coverage (100% of endpoints)
- Security scan results (zero critical vulnerabilities)
- Penetration test results

#### Implementation Details
- Use parameterized queries for database access
- Implement input validation attributes
- Use Content Security Policy (CSP) headers
- Implement rate limiting and request throttling

---

### NFR-SEC-006: API Security
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Security

#### Description
The system shall secure all API endpoints against common attacks.

#### Acceptance Criteria
- Implement CORS policy (whitelist allowed origins)
- Rate limiting: 100 requests per minute per client
- API versioning for backward compatibility
- Implement API key rotation mechanism
- Protect against CSRF attacks

#### Measurement Criteria
- API security scan results
- Rate limit effectiveness
- CORS policy compliance

#### Implementation Details
- Configure CORS middleware with strict policy
- Implement token bucket rate limiting
- Use anti-forgery tokens for state-changing operations
- Implement API gateway for centralized security

---

## Reliability Requirements

### NFR-REL-001: Availability
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Reliability

#### Description
The system shall be available for use with minimal downtime.

#### Acceptance Criteria
- System uptime: 99.9% (< 8.76 hours downtime per year)
- Planned maintenance windows < 4 hours per month
- Graceful degradation during partial failures
- Automatic recovery from transient failures

#### Measurement Criteria
- Uptime percentage
- Mean Time Between Failures (MTBF)
- Mean Time To Recovery (MTTR)

#### Implementation Details
- Implement health check endpoints
- Use circuit breaker pattern for external dependencies
- Implement retry logic with exponential backoff
- Deploy to multiple availability zones

---

### NFR-REL-002: Fault Tolerance
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Reliability

#### Description
The system shall continue operating in the presence of failures.

#### Acceptance Criteria
- Handle database connection failures gracefully
- Recover from network timeouts
- Continue operation if external services are unavailable
- Preserve data integrity during failures
- No data loss during system crashes

#### Measurement Criteria
- Failure recovery success rate
- Data integrity verification
- System resilience under failure conditions

#### Implementation Details
- Implement database connection retry logic
- Use message queues for asynchronous operations
- Implement transaction management with rollback
- Use distributed tracing for failure diagnosis

---

### NFR-REL-003: Data Backup and Recovery
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Reliability

#### Description
The system shall backup data regularly and support disaster recovery.

#### Acceptance Criteria
- Automated daily backups
- Backup retention: 30 days
- Recovery Point Objective (RPO): < 24 hours
- Recovery Time Objective (RTO): < 4 hours
- Backup integrity verification
- Documented recovery procedures

#### Measurement Criteria
- Backup success rate (100%)
- Backup restoration test results
- RPO/RTO compliance

#### Implementation Details
- Implement automated backup scripts
- Store backups in geographically separate location
- Test backup restoration quarterly
- Document disaster recovery runbook

---

### NFR-REL-004: Error Handling
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Reliability

#### Description
The system shall handle errors gracefully and provide meaningful error messages.

#### Acceptance Criteria
- Catch and handle all exceptions
- Return appropriate HTTP status codes
- Provide user-friendly error messages
- Log detailed error information for debugging
- No sensitive information in error messages

#### Measurement Criteria
- Unhandled exception rate (target: 0)
- Error message clarity (user feedback)
- Error resolution time

#### Implementation Details
- Implement global exception handling middleware
- Use custom exception types for business logic errors
- Implement error response standardization
- Use correlation IDs for error tracking

---

## Scalability Requirements

### NFR-SCAL-001: Horizontal Scalability
**Priority**: Medium  
**Status**: Should Implement  
**Quality Attribute**: Scalability

#### Description
The system shall support horizontal scaling to handle increased load.

#### Acceptance Criteria
- Support deployment across multiple instances
- Stateless application design
- Load balancing across instances
- Session state stored externally (Redis, database)
- Scale from 1 to 10 instances without code changes

#### Measurement Criteria
- Linear performance scaling with instance count
- Load distribution across instances
- Session affinity handling

#### Implementation Details
- Design stateless API endpoints
- Use distributed cache for shared state
- Implement sticky sessions if needed
- Use container orchestration (Kubernetes, Docker Swarm)

---

### NFR-SCAL-002: Data Volume Scalability
**Priority**: Medium  
**Status**: Should Implement  
**Quality Attribute**: Scalability

#### Description
The system shall handle growing data volumes efficiently.

#### Acceptance Criteria
- Support 10,000+ cloud resources
- Handle 1 million+ historical data points
- Efficient queries on large datasets
- Data archival strategy for old data
- Partition large tables for performance

#### Measurement Criteria
- Query performance with increasing data volume
- Storage growth rate
- Data archival effectiveness

#### Implementation Details
- Implement database indexing strategy
- Use data partitioning/sharding
- Implement data archival jobs
- Use pagination for large result sets

---

### NFR-SCAL-003: Geographic Distribution
**Priority**: Low  
**Status**: Could Implement  
**Quality Attribute**: Scalability

#### Description
The system shall support deployment across multiple geographic regions.

#### Acceptance Criteria
- Support multi-region deployment
- Data replication across regions
- Region-aware routing
- Compliance with data residency requirements

#### Measurement Criteria
- Cross-region latency
- Data replication lag
- Regional failover time

#### Implementation Details
- Use geo-distributed database
- Implement CDN for static assets
- Use DNS-based routing
- Implement data residency controls

---

## Maintainability Requirements

### NFR-MAINT-001: Code Quality
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Maintainability

#### Description
The system shall maintain high code quality standards for long-term maintainability.

#### Acceptance Criteria
- Code coverage > 80% for unit tests
- Zero critical code quality issues (SonarQube)
- Follow C# coding conventions
- Use consistent naming conventions
- Maximum cyclomatic complexity: 10 per method

#### Measurement Criteria
- Code coverage percentage
- Code quality metrics (maintainability index, cyclomatic complexity)
- Technical debt ratio

#### Implementation Details
- Implement automated code quality checks
- Use static code analysis tools
- Enforce code review process
- Use linting and formatting tools

---

### NFR-MAINT-002: Documentation
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Maintainability

#### Description
The system shall be well-documented for developers and operators.

#### Acceptance Criteria
- API documentation (OpenAPI/Swagger)
- Code documentation (XML comments)
- Architecture documentation
- Deployment guide
- Operations runbook
- Troubleshooting guide

#### Measurement Criteria
- Documentation coverage
- Documentation accuracy (verified quarterly)
- Time to onboard new developers

#### Implementation Details
- Generate API docs from code annotations
- Use Swagger UI for interactive API docs
- Maintain architecture decision records (ADRs)
- Document deployment procedures

---

### NFR-MAINT-003: Modularity
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Maintainability

#### Description
The system shall be designed with modular, loosely coupled components.

#### Acceptance Criteria
- Clear separation of concerns (MVC pattern)
- Service layer abstraction
- Dependency injection for loose coupling
- Interface-based design
- Single Responsibility Principle (SRP) compliance

#### Measurement Criteria
- Coupling metrics (afferent/efferent coupling)
- Cohesion metrics
- Component independence

#### Implementation Details
- Use ASP.NET Core dependency injection
- Define service interfaces
- Implement repository pattern for data access
- Use SOLID principles

---

### NFR-MAINT-004: Testability
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Maintainability

#### Description
The system shall be designed to facilitate automated testing.

#### Acceptance Criteria
- Unit test coverage > 80%
- Integration test coverage for critical paths
- Automated test execution in CI/CD pipeline
- Mock external dependencies in tests
- Test data management strategy

#### Measurement Criteria
- Test coverage percentage
- Test execution time
- Test failure rate

#### Implementation Details
- Use xUnit or NUnit for unit tests
- Use Moq for mocking dependencies
- Implement test fixtures and helpers
- Use in-memory database for integration tests

---

### NFR-MAINT-005: Logging and Monitoring
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Maintainability

#### Description
The system shall provide comprehensive logging and monitoring capabilities.

#### Acceptance Criteria
- Structured logging (JSON format)
- Log levels: Debug, Info, Warning, Error, Critical
- Application performance monitoring (APM)
- Health check endpoints
- Metrics collection (requests, errors, latency)
- Distributed tracing support

#### Measurement Criteria
- Log completeness
- Monitoring coverage
- Mean Time To Detect (MTTD) issues

#### Implementation Details
- Use Serilog or NLog for structured logging
- Implement Application Insights or similar APM
- Use correlation IDs for request tracing
- Implement custom metrics collection

---

## Usability Requirements

### NFR-USE-001: User Interface Design
**Priority**: Medium  
**Status**: Should Implement  
**Quality Attribute**: Usability

#### Description
The system shall provide an intuitive and user-friendly interface.

#### Acceptance Criteria
- Responsive design (mobile, tablet, desktop)
- Consistent UI/UX across all pages
- Accessibility compliance (WCAG 2.1 Level AA)
- Clear navigation and information hierarchy
- Loading indicators for long operations

#### Measurement Criteria
- User satisfaction score (> 4.0/5.0)
- Task completion rate (> 90%)
- Time to complete common tasks

#### Implementation Details
- Use modern CSS framework (Bootstrap, Tailwind)
- Implement responsive grid layout
- Use semantic HTML
- Implement ARIA attributes for accessibility

---

### NFR-USE-002: Error Messages
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Usability

#### Description
The system shall provide clear, actionable error messages to users.

#### Acceptance Criteria
- User-friendly error messages (no technical jargon)
- Suggest corrective actions
- Consistent error message format
- Appropriate error severity indication
- Context-specific help links

#### Measurement Criteria
- User comprehension of error messages
- Error resolution time
- Support ticket reduction

#### Implementation Details
- Define error message templates
- Implement error message localization
- Provide contextual help
- Use appropriate HTTP status codes

---

### NFR-USE-003: Dashboard and Visualization
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Usability

#### Description
The system shall provide effective data visualization for cost and waste analysis.

#### Acceptance Criteria
- Interactive charts and graphs
- Drill-down capability for detailed analysis
- Export data to CSV/Excel
- Customizable dashboard widgets
- Real-time data updates

#### Measurement Criteria
- Dashboard load time (< 3 seconds)
- User engagement with visualizations
- Data export usage

#### Implementation Details
- Use Chart.js or D3.js for visualizations
- Implement responsive charts
- Use color-blind friendly color schemes
- Implement data export functionality

---

### NFR-USE-004: Help and Documentation
**Priority**: Medium  
**Status**: Should Implement  
**Quality Attribute**: Usability

#### Description
The system shall provide comprehensive help and documentation for users.

#### Acceptance Criteria
- Context-sensitive help
- User guide and tutorials
- FAQ section
- Video tutorials for common tasks
- In-app tooltips and hints

#### Measurement Criteria
- Help documentation usage
- Support ticket reduction
- User self-service rate

#### Implementation Details
- Implement help widget
- Create user documentation
- Record video tutorials
- Implement interactive tours

---

## Compatibility Requirements

### NFR-COMP-001: Browser Compatibility
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Compatibility

#### Description
The system shall support modern web browsers.

#### Acceptance Criteria
- Support Chrome (latest 2 versions)
- Support Firefox (latest 2 versions)
- Support Edge (latest 2 versions)
- Support Safari (latest 2 versions)
- Graceful degradation for older browsers

#### Measurement Criteria
- Browser compatibility test results
- User browser distribution
- Browser-specific issues

#### Implementation Details
- Use progressive enhancement
- Test on target browsers
- Use polyfills for missing features
- Implement feature detection

---

### NFR-COMP-002: API Versioning
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Compatibility

#### Description
The system shall support API versioning for backward compatibility.

#### Acceptance Criteria
- URL-based versioning (e.g., /api/v1/)
- Support at least 2 API versions concurrently
- Deprecation notice period: 6 months
- Clear migration guide for version changes

#### Measurement Criteria
- API version adoption rate
- Breaking change frequency
- Client migration success rate

#### Implementation Details
- Implement API versioning middleware
- Document version differences
- Provide migration tools
- Use semantic versioning

---

### NFR-COMP-003: Integration Compatibility
**Priority**: Medium  
**Status**: Should Implement  
**Quality Attribute**: Compatibility

#### Description
The system shall integrate with common cloud platforms and tools.

#### Acceptance Criteria
- Support AWS Cost Explorer API
- Support Azure Cost Management API
- Support GCP Cloud Billing API
- Support webhook integrations
- Support SAML/OAuth for SSO

#### Measurement Criteria
- Integration success rate
- API compatibility maintenance
- Integration error rate

#### Implementation Details
- Use official cloud provider SDKs
- Implement adapter pattern for integrations
- Version lock external dependencies
- Test integrations regularly

---

## Compliance Requirements

### NFR-COMP-001: Data Privacy
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Compliance

#### Description
The system shall comply with data privacy regulations (GDPR, CCPA).

#### Acceptance Criteria
- Implement data subject rights (access, deletion, portability)
- Obtain user consent for data processing
- Provide privacy policy
- Implement data retention policies
- Support data anonymization

#### Measurement Criteria
- Privacy compliance audit results
- Data subject request response time
- Privacy policy acceptance rate

#### Implementation Details
- Implement data export functionality
- Implement data deletion functionality
- Use consent management
- Document data processing activities

---

### NFR-COMP-002: Security Standards
**Priority**: High  
**Status**: Must Implement  
**Quality Attribute**: Compliance

#### Description
The system shall comply with security standards and best practices.

#### Acceptance Criteria
- OWASP Top 10 compliance
- CIS Benchmarks compliance
- Regular security assessments
- Vulnerability scanning
- Penetration testing annually

#### Measurement Criteria
- Security scan results (zero critical vulnerabilities)
- Compliance audit results
- Security incident count

#### Implementation Details
- Implement security scanning in CI/CD
- Conduct regular security audits
- Maintain security documentation
- Implement security training

---

### NFR-COMP-003: Audit and Compliance Reporting
**Priority**: Medium  
**Status**: Should Implement  
**Quality Attribute**: Compliance

#### Description
The system shall support audit and compliance reporting requirements.

#### Acceptance Criteria
- Generate compliance reports
- Audit trail for all data changes
- Support compliance frameworks (SOC 2, ISO 27001)
- Retain audit logs per regulatory requirements
- Provide audit log export

#### Measurement Criteria
- Audit log completeness
- Compliance report accuracy
- Audit readiness

#### Implementation Details
- Implement comprehensive audit logging
- Create compliance report templates
- Document compliance controls
- Conduct regular compliance reviews

---

## Performance Benchmarks

### System Performance Targets

| Metric | Target | Measurement Method |
|--------|--------|-------------------|
| API Response Time (p95) | < 2 seconds | APM monitoring |
| API Response Time (p99) | < 5 seconds | APM monitoring |
| Dashboard Load Time | < 3 seconds | Browser timing API |
| Resource Analysis Time | < 5 seconds | Application logs |
| Concurrent Users | 100+ | Load testing |
| Requests per Minute | 1000+ | Load testing |
| Database Query Time | < 100ms | Database profiling |
| Memory Usage | < 2GB | System monitoring |
| CPU Utilization | < 70% | System monitoring |
| System Uptime | 99.9% | Uptime monitoring |

---

## Quality Attributes Summary

| Quality Attribute | Priority | Key Requirements |
|------------------|----------|------------------|
| Performance | High | Response time, throughput, resource utilization |
| Security | High | Authentication, authorization, encryption, audit logging |
| Reliability | High | Availability, fault tolerance, backup/recovery |
| Scalability | Medium | Horizontal scaling, data volume handling |
| Maintainability | High | Code quality, documentation, modularity, testability |
| Usability | Medium | UI design, error messages, visualization |
| Compatibility | High | Browser support, API versioning, integrations |
| Compliance | High | Data privacy, security standards, audit reporting |

---

## Testing Strategy

### Performance Testing
- Load testing with 100+ concurrent users
- Stress testing to identify breaking points
- Endurance testing for 24+ hours
- Spike testing for sudden load increases

### Security Testing
- Vulnerability scanning (weekly)
- Penetration testing (annually)
- Security code review
- Dependency vulnerability scanning

### Reliability Testing
- Chaos engineering (failure injection)
- Disaster recovery drills (quarterly)
- Backup restoration testing (quarterly)
- Failover testing

### Usability Testing
- User acceptance testing (UAT)
- A/B testing for UI changes
- Accessibility testing
- Cross-browser testing

---

## Monitoring and Alerting

### Key Metrics to Monitor
- Application performance (response time, throughput)
- System resources (CPU, memory, disk)
- Error rates and types
- User activity and engagement
- Security events and anomalies
- Business metrics (cost savings, waste detected)

### Alert Thresholds
- Response time > 5 seconds (warning)
- Error rate > 1% (critical)
- CPU utilization > 80% (warning)
- Memory usage > 90% (critical)
- System downtime (critical)
- Security events (critical)

---

## Success Criteria

### Technical Success Metrics
- All high-priority NFRs implemented
- Performance targets met consistently
- Zero critical security vulnerabilities
- 99.9% system uptime achieved
- Code quality metrics within targets

### Business Success Metrics
- User satisfaction score > 4.0/5.0
- System adoption rate > 80%
- Support ticket reduction > 30%
- Time to value < 1 week
- ROI positive within 6 months

---

*Document Version: 1.0*  
*Last Updated: 2026-05-22*  
*Based on: cloudoptimizer-requirements-schema.jsonld*