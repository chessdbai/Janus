import * as cdk from '@aws-cdk/core';
import * as accounts from '@chessdb.biz/zeus-accounts';
import { Config } from './config';

export class AppStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, account: accounts.ZeusServiceAccount) {
    super(scope, id, {
      env: account.environment
    });

    new Config(this, 'Config', {
      stage: account.stage
    });
  }
}
