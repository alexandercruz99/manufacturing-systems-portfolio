# Security Policy

## Never Commit Secrets

**This repository must never contain:**
- API keys or tokens
- Passwords or credentials
- Database connection strings with credentials
- Private endpoints or internal URLs
- Personal information or PII

## If You Find a Secret

1. **Immediately** remove it from the repository
2. Rotate/revoke the exposed secret
3. Add it to `.gitignore`
4. Create a `.env.example` file showing the required variables (without values)
5. Document the change in a commit message

## Reporting Security Issues

If you discover a security vulnerability, please report it responsibly rather than opening a public issue.

