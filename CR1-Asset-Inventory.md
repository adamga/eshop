# PCI DSS Control Objective 1: Critical Asset Inventory

This document lists and classifies the critical assets for the eShop application as required by PCI DSS Control Objective 1.

| Asset Name                | Description                                      | Location/Service                        | Classification      |
|--------------------------|--------------------------------------------------|-----------------------------------------|---------------------|
| Payment Data              | Cardholder data, payment tokens, transaction info| Basket.API, Ordering.API, PaymentProcessor | Sensitive         |
| User Credentials          | Usernames, passwords, tokens                     | Identity.API, ClientApp, WebApp         | Sensitive          |
| Cryptographic Keys        | Keys for encryption, signing, authentication     | appsettings.json, environment variables | Sensitive          |
| Configuration Files       | Application settings, connection strings         | appsettings.json, appsettings.Development.json | Confidential |
| Business Logic            | Payment, order, and basket processing logic      | Catalog.API, Ordering.API, PaymentProcessor | Confidential   |
| User Data                 | Personal info, addresses, order history          | Basket.API, Ordering.API, WebApp        | Confidential       |
| Integration Events        | Event payloads for inter-service communication   | IntegrationEvents folders                | Confidential       |
| Logs                      | Application and security logs                    | Various /logs/ folders, external logging | Confidential       |
| Third-party API Keys      | External service credentials                     | appsettings.json, environment variables | Sensitive          |

> **Note:** This inventory should be reviewed and updated regularly as the application evolves.
