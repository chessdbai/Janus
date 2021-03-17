#!/bin/bash
set -euxo pipefail

npx cdk deploy \
  --profile chessdb-deploy \
  DeployStack