#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from '@aws-cdk/core';
import * as pipelines from '@aws-cdk/pipelines';
import * as stacks from '../lib';
import AccountManager, { ZeusServiceAccount, ZeusCorpAccount } from '@chessdb.biz/zeus-accounts';
import * as iam from '@aws-cdk/aws-iam';
import * as codepipeline from '@aws-cdk/aws-codepipeline';
import * as codepipeline_actions from '@aws-cdk/aws-codepipeline-actions';

class Janus extends cdk.Stage {
  constructor(scope: cdk.Construct, id: string, account: ZeusServiceAccount) {
    super(scope, id, {
      env: account.environment
    });
  
    const appStack = new stacks.AppStack(this, 'AppStack', account);
  }
}

const betaAccount = AccountManager.getAccounts({
  stages: [ 'Beta' ],
  tag: 'Service'
})[0] as ZeusServiceAccount;
const prodAccount = AccountManager.getAccounts({
  stages: [ 'Prod' ],
  tag: 'Service'
})[0] as ZeusServiceAccount;
const deployAccount = AccountManager.getAccounts({
  tag: 'Deployment'
})[0] as ZeusCorpAccount;

const synthezier = new cdk.DefaultStackSynthesizer({
  
});

/**
 * Stack to hold the pipeline
 */
 class DeployStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    const sourceArtifact = new codepipeline.Artifact();
    const cloudAssemblyArtifact = new codepipeline.Artifact();

    const pipeline = new pipelines.CdkPipeline(this, 'Pipeline', {
      pipelineName: 'Janus',
      cloudAssemblyArtifact,
      sourceAction: new codepipeline_actions.GitHubSourceAction({
        actionName: 'GitHub',
        output: sourceArtifact,
        oauthToken: cdk.SecretValue.secretsManager('corp/Deploy/GitHub'),
        // Replace these with your actual GitHub project name
        owner: 'chessdbai',
        repo: 'Janus',
        branch: 'master', // default: 'master'
      }),

      synthAction: pipelines.SimpleSynthAction.standardNpmSynth({
        sourceArtifact,
        cloudAssemblyArtifact,
        buildCommand: 'npm run build',
        installCommand: 'npm run bootstrap',
        rolePolicyStatements: [
          new iam.PolicyStatement({
            actions: [
              "sts:GetServiceBearerToken",
              "codeartifact:GetPackageVersionReadme",
              "codeartifact:GetAuthorizationToken",
              "codeartifact:DescribeRepository",
              "codeartifact:ReadFromRepository",
              "codeartifact:GetRepositoryEndpoint",
              "codeartifact:DescribeDomain",
              "codeartifact:DescribePackageVersion",
              "codeartifact:GetPackageVersionAsset",
              "codeartifact:GetRepositoryPermissionsPolicy",
              "codeartifact:GetDomainPermissionsPolicy"
            ],
            resources: ['*']
          })
        ]
      }),
    });

    pipeline.addApplicationStage(new Janus(this, 'Beta', betaAccount), {
      
    });
    pipeline.addApplicationStage(new Janus(this, 'Prod', prodAccount));
  }
}

const app = new cdk.App();
new DeployStack(app, 'DeployStack', {
  env: {
    account: deployAccount.accountId,
    region: deployAccount.region
  }
});