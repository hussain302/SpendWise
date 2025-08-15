# 🚀 SpendWise

**SpendWise** is a **real-world ready template** for building scalable, production-grade **.NET 8 applications** with **Clean Architecture**, **CQRS**, and battle-tested best practices.  
Instead of starting from scratch, you get a **solid foundation** packed with integrations, performance optimizations, and maintainable patterns.

---

## ✨ Features

### 🔧 Architecture & Core Patterns
- **Clean Architecture** – maintainable, modular, and testable.
- **CQRS** – clear separation of read and write operations.
- **Unit of Work & Repository Patterns** – organized and consistent data access.
- **EF Core + Dapper** – flexibility in persistence strategies.

### 🗄️ Database & Caching
- Supports **SQL Server** & **PostgreSQL**.
- Integrated **Redis** for high-speed caching.

### 🛠 Middleware & Processing
- Custom middleware for cross-cutting concerns.
- Unified error handling with standard HTTP codes.
- **BaseResult Pattern** – consistent API responses.
- Background tasks & messaging via **Hangfire** & **RabbitMQ**.

### 📊 Scalability & Monitoring
- **YARP Reverse Proxy** for load balancing.
- Health check endpoints.
- **Prometheus + Grafana** dashboards for real-time metrics.

### 🔐 API & Security
- Minimal APIs with **Options Pattern**.
- Authentication with **JWT & OAuth**.
- Rate limiting, idempotency, and feature toggles.

### 📋 Developer Experience
- Fully configured **Swagger** & **Scalar** (security, examples, versioning).
- Enum list handlers, custom pagination, and exception utilities.
- Encryption/Decryption helpers, CAPTCHA generator, and more.

---

## 📂 Folder Structure
SpendWise/
│
├── src/ # Application source code
│ ├── Api # API Layer
│ ├── Application # CQRS, DTOs, Services
│ ├── Domain # Entities & Interfaces
│ ├── Infrastructure # DB, Caching, External Integrations
│
├── tests/ # Unit & Integration Tests
│
├── docs/ # Documentation & Guides
│
└── docker/ # Docker & Compose configs

## 🛠 Getting Started

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/hussain302/SpendWise.git
cd SpendWise

2️⃣ Configure Environment

Copy .env.example to .env and update variables as needed.

3️⃣ Run with Docker
docker-compose up -d

4️⃣ Access the API

Swagger: http://localhost:5000/swagger

Health Check: http://localhost:5000/health

🖥 Technology Stack

.NET 8

Entity Framework Core & Dapper

SQL Server / PostgreSQL

Redis

Hangfire / RabbitMQ

Prometheus + Grafana

YARP Reverse Proxy

🤝 Contributing

Contributions are welcome!
Feel free to fork, submit pull requests, and open issues.

📜 License

This project is licensed under the MIT License.

🌟 Star the Repo

If you find SpendWise helpful, consider giving it a ⭐ on GitHub to show support.
