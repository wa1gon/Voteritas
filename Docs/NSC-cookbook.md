### Create Operator
1. nsc add operator --generate-signing-key --sys --name Arkansas
   - Create an operator called Arkansas
   - It is a system account
   - It is also a signing account
2. nsc edit operator --require-signing-keys --account-jwt-server-url "nats://0.0.0.0:4222"
   - All Accounts are required to be signing account
   - Set up the server for JWT Location
### Creating Accounts
1. nsc add account Benton
   - Add an account for Benton County
3. nsc edit account Benton --sk
### Create User
1. nsc add user Voter_1
### Admin commands
1. nsc list keys -A
2. nsc generate config --nats-resolver --sys-account SYS > resolver.conf
3. nats-server -c resolver.conf
4. nsc push -A
### Create NATS context
1. nats context save Arkansas_sys --nsc "nsc://Arkansas/SYS/sys"
2. nats context save Arkansas_Voter_1 --nsc "nsc://Arkansas/Benton/Voter_1"

   
