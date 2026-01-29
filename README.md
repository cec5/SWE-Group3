# SWE-Group3

## Pre-commit Hooks

This project uses [pre-commit](https://pre-commit.com/) to enforce code and file hygiene before committing. To set it up locally:

Check you have Python/Pip installed:
```bash
python3 --version
pip --version
```
Now navigate to the repository root and run:
```bash
pip install pre-commit
pre-commit install
pre-commit run --all-files
```
