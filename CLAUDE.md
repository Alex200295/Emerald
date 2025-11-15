# CLAUDE.md - AI Assistant Guide for Emerald

## Project Overview

**Project Name:** Emerald
**License:** Apache License 2.0
**Repository Status:** Initial setup phase
**Primary Branch:** (to be determined)

### Purpose
This document provides AI assistants with essential context about the Emerald codebase, development workflows, conventions, and best practices.

---

## Repository Structure

### Current State
```
Emerald/
├── .git/               # Git version control
├── LICENSE             # Apache License 2.0
├── README.md           # Project overview
└── CLAUDE.md           # This file - AI assistant guide
```

### Expected Future Structure
As the project develops, the following structure is recommended:

```
Emerald/
├── .git/
├── .github/            # GitHub workflows, issue templates, PR templates
│   └── workflows/      # CI/CD pipelines
├── docs/               # Documentation
│   ├── architecture/   # Architecture decision records
│   ├── api/           # API documentation
│   └── guides/        # User and developer guides
├── src/               # Source code
│   ├── core/          # Core functionality
│   ├── utils/         # Utility functions
│   └── tests/         # Test files
├── scripts/           # Build and automation scripts
├── config/            # Configuration files
├── LICENSE
├── README.md
├── CLAUDE.md
└── [build config]     # package.json, Cargo.toml, etc. (language-specific)
```

---

## Development Workflows

### Git Workflow

#### Branch Naming Convention
- **Feature branches:** `feature/<descriptive-name>`
- **Bug fixes:** `bugfix/<issue-number>-<description>`
- **Hot fixes:** `hotfix/<description>`
- **Releases:** `release/<version>`
- **Claude AI branches:** `claude/claude-md-<session-id>`

#### Commit Message Guidelines
Follow the Conventional Commits specification:

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, missing semicolons, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks
- `perf`: Performance improvements
- `ci`: CI/CD changes

**Example:**
```
feat(auth): add OAuth2 authentication support

Implement OAuth2 flow with support for Google and GitHub providers.
Includes token refresh mechanism and session management.

Closes #123
```

#### Pull Request Process
1. Create feature branch from main branch
2. Make changes with clear, atomic commits
3. Write/update tests for new functionality
4. Update documentation as needed
5. Ensure all tests pass and code is linted
6. Create PR with descriptive title and summary
7. Address review feedback
8. Squash commits if needed before merge

### Code Review Standards
- All code changes require review before merging
- Check for:
  - Code correctness and logic
  - Test coverage
  - Documentation updates
  - Performance implications
  - Security vulnerabilities (OWASP Top 10)
  - Code style consistency

---

## Code Conventions

### General Principles
1. **Clarity over cleverness:** Write code that is easy to understand
2. **DRY (Don't Repeat Yourself):** Avoid code duplication
3. **SOLID principles:** Follow object-oriented design principles
4. **Separation of concerns:** Keep related code together, unrelated code apart
5. **Fail fast:** Validate inputs early and provide clear error messages

### Security Best Practices
AI assistants must be vigilant about security when writing code:

#### Common Vulnerabilities to Avoid
1. **Injection Attacks**
   - SQL Injection: Use parameterized queries/prepared statements
   - Command Injection: Validate and sanitize all user inputs
   - XSS: Escape output, use Content Security Policy

2. **Authentication & Authorization**
   - Never hardcode credentials
   - Use environment variables for sensitive data
   - Implement proper session management
   - Use secure password hashing (bcrypt, Argon2)

3. **Data Exposure**
   - Don't log sensitive information
   - Implement proper access controls
   - Use HTTPS for all communications
   - Sanitize error messages shown to users

4. **Dependency Management**
   - Keep dependencies up to date
   - Audit for known vulnerabilities
   - Use lock files to ensure reproducible builds

### Code Documentation
- Use clear, descriptive names for variables, functions, and classes
- Add comments for complex logic or non-obvious behavior
- Include docstrings/JSDoc for public APIs
- Keep comments up-to-date with code changes

### Error Handling
- Use appropriate error types for different scenarios
- Provide context in error messages
- Log errors with sufficient detail for debugging
- Never expose internal implementation details in user-facing errors

---

## Testing Strategy

### Test Coverage Goals
- Aim for >80% code coverage
- 100% coverage for critical paths
- All public APIs must have tests

### Test Types
1. **Unit Tests**
   - Test individual functions/methods in isolation
   - Fast execution
   - No external dependencies

2. **Integration Tests**
   - Test component interactions
   - May use test databases or mock services

3. **End-to-End Tests**
   - Test complete user workflows
   - Run in environment similar to production

### Test Naming Convention
```
test_<function_name>_<scenario>_<expected_result>
```

Example: `test_user_login_with_invalid_credentials_returns_error`

### Running Tests
```bash
# Run all tests
[test command for the project's language]

# Run specific test file
[language-specific command]

# Run with coverage
[coverage command]
```

---

## Documentation Standards

### README.md
Must include:
- Project description and purpose
- Installation instructions
- Quick start guide
- Basic usage examples
- Link to full documentation
- Contributing guidelines
- License information

### Code Documentation
- Document all public APIs
- Include usage examples
- Describe parameters, return values, and exceptions
- Note any side effects or special behaviors

### Architecture Documentation
- Document major architectural decisions
- Use ADRs (Architecture Decision Records) for significant choices
- Keep diagrams up-to-date with implementation

---

## AI Assistant Guidelines

### When Working on This Project

#### Before Making Changes
1. **Understand the context:** Read existing code and documentation
2. **Check for patterns:** Follow established conventions in the codebase
3. **Verify requirements:** Ensure you understand what's being asked
4. **Plan the approach:** Think through the implementation strategy

#### While Making Changes
1. **Follow conventions:** Use the code style and patterns already present
2. **Write tests:** Add tests for new functionality
3. **Update docs:** Keep documentation synchronized with code
4. **Security check:** Review code for common vulnerabilities
5. **Use appropriate tools:** Prefer specialized tools over bash commands

#### After Making Changes
1. **Test thoroughly:** Run all tests and verify functionality
2. **Review your work:** Check for potential issues
3. **Clean commit history:** Make atomic, well-described commits
4. **Update CLAUDE.md:** If you discover new conventions or patterns, document them here

### Code Quality Checklist
Before committing changes, verify:
- [ ] Code follows project conventions
- [ ] Tests are written and passing
- [ ] Documentation is updated
- [ ] No security vulnerabilities introduced
- [ ] No hardcoded secrets or credentials
- [ ] Error handling is appropriate
- [ ] Code is readable and well-commented
- [ ] No unnecessary dependencies added
- [ ] Backward compatibility maintained (or breaking changes documented)

### Communication Style
- Be concise and technical in commit messages
- Provide clear explanations in PR descriptions
- Reference related issues and PRs
- Include examples when documenting new features

---

## Environment Setup

### Prerequisites
(To be determined based on project language/framework)

### Installation
```bash
# Clone the repository
git clone <repository-url>
cd Emerald

# Install dependencies
[installation commands to be added]

# Set up environment
[environment setup to be added]
```

### Configuration
- Use `.env` files for local configuration (never commit these)
- Use `.env.example` as a template
- Document all required environment variables

---

## Common Tasks

### Adding a New Feature
1. Create a feature branch: `git checkout -b feature/feature-name`
2. Implement the feature with tests
3. Update documentation
4. Create a pull request
5. Address review feedback
6. Merge after approval

### Fixing a Bug
1. Create a bugfix branch: `git checkout -b bugfix/issue-number-description`
2. Write a failing test that reproduces the bug
3. Fix the bug
4. Verify the test passes
5. Create a pull request with reference to the issue

### Refactoring
1. Ensure comprehensive test coverage exists
2. Make incremental changes
3. Run tests after each change
4. Keep refactoring separate from feature work
5. Document significant changes

---

## Troubleshooting

### Common Issues
(To be populated as issues arise)

### Getting Help
- Check existing documentation in `/docs`
- Review similar code in the codebase
- Check issue tracker for related problems
- Consult project README for contact information

---

## Project-Specific Notes

### Current Status
The Emerald project is in its initial setup phase. This document will be updated as the project structure and conventions are established.

### Next Steps
When the project develops, update this document with:
1. Chosen programming language and framework
2. Build and test commands
3. Deployment procedures
4. API documentation locations
5. Specific coding standards for the language
6. CI/CD pipeline details

---

## Changelog

### 2025-11-15
- Initial creation of CLAUDE.md
- Established baseline structure and conventions
- Added security guidelines and best practices

---

## References

- [Conventional Commits](https://www.conventionalcommits.org/)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [Semantic Versioning](https://semver.org/)
- [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0)

---

**Last Updated:** 2025-11-15
**Version:** 1.0.0
**Maintained by:** AI Assistants working on Emerald
