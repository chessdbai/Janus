DOCKERPWD=$(aws ecr get-login-password --region us-east-2)
sudo docker login --username AWS --password $DOCKERPWD 407299974961.dkr.ecr.us-east-2.amazonaws.com
sudo docker pull 407299974961.dkr.ecr.us-east-2.amazonaws.com/maki:1.1.0.2