import * as cdk from '@aws-cdk/core';
import * as appconfig from '@aws-cdk/aws-appconfig';

interface ConfigProps {
  stage: string
}

export class Config extends cdk.Construct {

  constructor(parent: cdk.Construct, name: string, props: ConfigProps) {
    super(parent, name);

    const janusApp = new appconfig.CfnApplication(this, 'JanusApp', {
      name: 'Janus'
    });

    const janusEnv = new appconfig.CfnEnvironment(this, 'JanusEnv', {
      name: props.stage, 
      applicationId: janusApp.ref
    });
  }
}