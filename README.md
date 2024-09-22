# Trusted Voting System Using PKI - README

## Overview

This project implements a trusted voting system using Public Key Infrastructure
(PKI) to securely sign and validate votes. The system is designed to ensure the
integrity of the election process by verifying voter identities, preventing
multiple votes, and maintaining the anonymity of ballots. The system is built
using C# and follows best practices for cryptography and PKI.

## Key Components

### 1. **Election Authority**
   - The Election Authority is the trusted entity (e.g., a state or club)
     responsible for creating and managing the root certificate used to sign
     voter certificates.
   - It issues certificates to Voter Registrars and ensures the integrity of
     the voting block.
   - Certificates issued by the Election Authority are trusted by all entities
     in the system.

### 2. **Voter Registrar**
   - The Voter Registrar is the election official responsible for creating and
     issuing certificates for individual voters.
   - Each Voter Registrar holds a certificate signed by the Election Authority,
     ensuring their trustworthiness.
   - Voter certificates are valid for a set period, typically 4 years, and
     contain identifying information such as the voter's GUID or serial number.

### 3. **Voter Certificate**
   - Each voter is assigned a unique **Voter Certificate** issued by the Voter
     Registrar.
   - This certificate includes identifying information and is valid for a set
     period, such as 4 years.
   - The Voter Certificate enables the voter to participate in the election and
     ensures they can vote only once.
   
### 4. **Ballot Certificate**
   - A **Ballot Certificate** is generated when a voter casts their vote.
   - The Ballot Certificate contains a public/private key pair used to sign the
     vote (the Eballot). This ensures that the ballot has not been tampered
     with.
   - No identifying information about the voter is stored in the Ballot
     Certificate, preserving voter anonymity.
   - Once the voter has voted, a record is stored in a database to prevent
     multiple voting.

### 5. **Eballot**
   - An **Eballot** is a digital ballot signed with the voter’s Ballot
     Certificate.
   - It contains the Ballot Certificate and is designed to be verifiable by
     anyone using standard PKI tools to confirm its integrity.
   - Each Eballot has a unique ID and is stored in a database for future
     reference.
   - The Eballot is printed out for the voter to keep as a record of their vote.
   - While the **Eballot Certificate** expires after the election deadline, the
     validity of the Eballot can still be checked indefinitely to ensure that
     the ballot hasn't been altered.

### 6. **Database**
   - The system uses a database to store all relevant information, including:
     - Records of Voter Certificates and Ballot Certificates.
     - A flag indicating whether a voter has cast their vote (to prevent
       duplicate voting).
     - Eballots are stored securely with their unique IDs for later retrieval
       and verification.

## Security Features

- **Voter Anonymity**: Voter information is not linked to the ballot, ensuring
  that no one can trace the vote back to the individual.
- **Tamper Detection**: Each Eballot is signed using the Ballot Certificate,
  ensuring that any alterations to the ballot can be detected.
- **Single Vote Enforcement**: Once a voter casts their vote, their status is
  recorded in the database to prevent multiple votes.
- **Expiration of Certificates**: Both Voter and Eballot Certificates expire
  after their designated periods, enhancing security while preserving the
  ability to validate votes in the future.
- **End-to-End Encryption**: All sensitive information, including voter data and
  ballots, is encrypted using PKI.

## Requirements

- **.NET 6.0 or later**
- **Database System**: The database system used for storing voters, ballots, and
  records can be SQL Server, MySQL, or any database supporting encryption and
  secure storage.
- Messaging system.  Currently, NATS is being used because of its lightweight nature and security
- Voter registration code that has their private key encrypted on it.  However, messages will be abstracted as
  much as possible.

## Setup and Usage

### 1. **Certificate Generation**
   - The Election Authority generates a root certificate.
   - Voter Registrars are issued certificates by the Election Authority.
   - Voter certificates are issued by Voter Registrars for all eligible voters.

### 2. **Voting Process**
   - Voters authenticate with their Voter Certificate.
   - Upon voting, a Ballot Certificate is generated.
   - The Eballot is signed with the private key of the Ballot Certificate and
     submitted.
   - The system records that the voter has cast their vote to prevent multiple
     voting.

### 3. **Eballot Validation**
   - After submission, anyone can validate the eballot by using PKI tools to
     check the integrity of the signature and confirm that the eballot has not
     been altered.
   - The system stores all eballots and allows future validation using the
     unique ID associated with each ballot.

## Running the Application

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd trusted-voting-system

# Security Issues in the Trusted Voting System

The trusted voting system described has several robust security features using 
PKI to ensure the integrity of votes. However, there are still some potential 
security concerns to address:

### 1. **Voter Certificate Theft or Compromise**
- **Risk**: If a voter's certificate is compromised (e.g., through phishing, 
  malware, or hacking), an attacker could impersonate the voter and cast a 
  fraudulent vote.
- **Mitigation**: Implement multi-factor authentication (MFA) to ensure 
  additional security when voters authenticate with their voter certificates.

### 2. **Insider Threats**
- **Risk**: Election officials, such as the Election Authority or Voter 
  Registrar, could potentially issue fraudulent certificates or manipulate 
  voting records. Insiders could issue extra voter certificates or alter 
  database records.
- **Mitigation**: Implement role-based access control (RBAC) and maintain 
  strict audit logs. Monitoring and logging every action related to certificate 
  issuance can mitigate insider threats.

### 3. **Voter Privacy and Anonymity**
- **Risk**: Even though the system separates ballots from voter identities, 
  metadata (e.g., voting times, voter registration logs) could theoretically 
  allow a vote to be traced back to a voter.
- **Mitigation**: Randomize vote casting times and reduce data retention to 
  minimize the risk of de-anonymizing voters. Homomorphic encryption could 
  further protect voter anonymity.

### 4. **Ballot Certificate Generation**
- **Risk**: If the process of generating the Ballot Certificate at the time of 
  voting is compromised (e.g., by weak or predictable keys), ballots could be 
  vulnerable to tampering.
- **Mitigation**: Use strong cryptographic algorithms (e.g., RSA, ECC) with 
  sufficient key lengths (2048-bit RSA or 256-bit ECC). Employ hardware 
  security modules (HSMs) to securely generate and store keys.

### 5. **Replay Attacks**
- **Risk**: An attacker could capture and resend a previously submitted eballot, 
  potentially altering it or casting multiple votes.
- **Mitigation**: Ensure the system uses cryptographic nonces and unique 
  identifiers for each ballot to detect replay attacks. Store votes securely to 
  prevent duplicate submissions.

### 6. **Database Security**
- **Risk**: The database storing eballots, voter participation records, and 
  Ballot Certificates could be a target for attackers. If compromised, attackers 
  could tamper with or delete election records.
- **Mitigation**: Encrypt the database at rest and in transit. Enforce access 
  controls and use frequent backups. Integrity checks or blockchain can ensure 
  record accuracy and detect tampering.

### 7. **Denial of Service (DoS) Attacks**
- **Risk**: A DoS attack could overwhelm the system with traffic or exploit 
  vulnerabilities, disrupting the voting process.
- **Mitigation**: Use rate-limiting, load balancing, and Distributed Denial of 
  Service (DDoS) mitigation techniques to maintain availability during election 
  periods.

### 8. **Expiration of Certificates**
- **Risk**: Voter or Ballot Certificates could expire prematurely, 
  unintentionally preventing legitimate voters from casting their votes or 
  invalidating valid votes.
- **Mitigation**: Use accurate time synchronization for certificates and notify 
  voters of expiration dates. Ensure election timelines are carefully managed 
  to avoid issues with certificate expiration.

### 9. **Trust in PKI Infrastructure**
- **Risk**: The PKI system itself is a single point of failure. If the Election 
  Authority's root certificate is compromised, the entire system could be 
  jeopardized.
- **Mitigation**: The root certificate and private key should be stored securely 
  using hardware security modules (HSMs). Continuous monitoring of the Election 
  Authority’s infrastructure is essential to detect any signs of tampering.

### 10. **Ballot Tampering After Submission**
- **Risk**: If attackers gain access to the eballot storage database, they could 
  modify or delete submitted ballots, effectively altering election results.
- **Mitigation**: Implement strong encryption and digital signatures for eballots. 
  Use auditing and checks, potentially leveraging blockchain, to ensure ballots 
  cannot be altered once submitted.

### 11. **Voter Registration Fraud**
- **Risk**: Attackers could fraudulently register multiple fake voters or 
  duplicate identities, allowing them to cast extra votes.
- **Mitigation**: Implement strong identity verification for voter registration, 
  with an audit trail for all registration activities. Using biometric or MFA 
  methods for registration can help ensure valid identities.

### 12. **Physical Security of Voting Machines**
- **Risk**: If voting machines or devices used to generate Ballot Certificates 
  are physically compromised, attackers could alter software or generate weak 
  keys.
- **Mitigation**: Secure voting machines physically, and digitally sign and 
  verify software updates to prevent tampering.

## Summary of Mitigation Strategies

- Implement **multi-factor authentication** for voters and officials.
- Employ **strict auditing** for certificate issuance and voting activities.
- **Randomize** metadata to obscure voter identity.
- Use **tamper-proof cryptographic protocols** such as blockchain for audit 
  trails.
- Ensure **database encryption** and enforce access controls.
- Secure the **PKI infrastructure** by using HSMs and
