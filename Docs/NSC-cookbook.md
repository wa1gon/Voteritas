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

### Creating Creds file
1. nsc generate creds -n Voter_1 > Voter_1.creds

### Importing and Exporting service or stream
1. nsc add export --account Benton --name [subject] --server
2. nsc add import -i // interactive
3. nsc push -A

### Edit User prevs
1. nsc edit user -n [user_name] -account Benton --allow-pub [subject] --allow-sub "_INBOX.>"
2. 

   
