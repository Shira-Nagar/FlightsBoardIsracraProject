# Flight Board Management System

## Overview
A real-time flight board management system featuring an ASP.NET Core backend and a React frontend. The system enables flight management, live-updating flight board, search, add, and delete flights, with a focus on code quality, modern architecture, and unit testing.

## Installation
### Prerequisites
- .NET 8.0 or higher
- Node.js 18+
- npm
- Docker (optional for quick setup)

### Backend Setup
```bash
cd FlightSolution/FlightsApi
# Restore dependencies (if needed)
dotnet restore
# Apply migrations (if needed)
dotnet ef database update
# Run the server
dotnet run
```

### Frontend Setup
```bash
cd flights-client
npm install
npm run dev
```

### Run with Docker (optional)
```bash
docker-compose up --build
```

## Usage
- Access the frontend at: http://localhost:5173
- Login with the default user: `admin / admin123`
- Add, delete, search, and filter flights by status and destination.
- All changes are updated in real-time (SignalR).

## API Documentation
- **Swagger UI:** [http://localhost:5000/swagger](http://localhost:5000/swagger) (when running backend directly)
- **Example Endpoints:**
  - `GET /api/flights` — Get all flights
  - `POST /api/flights` — Add a new flight
  - `DELETE /api/flights/{id}` — Delete a flight
  - `GET /api/flights/search?status=Boarding&destination=NYC` — Search flights

### Example: Get All Flights (curl)
```bash
curl http://localhost/api/flights
```

### Example: Add Flight (curl)
```bash
curl -X POST http://localhost/api/flights \
  -H "Content-Type: application/json" \
  -d '{
    "flightNumber": "1234",
    "departure": "TLV",
    "destination": "NYC",
    "departureTime": "2024-08-01T10:00:00",
    "arrivalTime": "2024-08-01T16:00:00",
    "gate": "A1"
  }'
```

### Example: Delete Flight (curl)
```bash
curl -X DELETE http://localhost/api/flights/1
```

### Example: Search Flights (curl)
```bash
curl "http://localhost/api/flights/search?status=Boarding&destination=NYC"
```

## Documentation
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [React Docs](https://react.dev/)
- [Redux Toolkit](https://redux-toolkit.js.org/)
- [TanStack Query](https://tanstack.com/query/latest)
- [SignalR](https://learn.microsoft.com/aspnet/core/signalr/introduction)

## Contributing
- Code contributions are welcome! Please submit a well-documented Pull Request.
- Ensure unit tests, documentation, and code standards are met.
- License: MIT (see below).

## License
MIT License. See LICENSE file for details.

## Development & Maintenance
- TDD: All business logic is covered by xUnit tests.
- To run tests: `dotnet test` in the Flight.Test directory.
- To run the backend: `dotnet run`.
- To run the frontend: `npm run dev`.
- For full stack: `docker-compose up`.

## Known Issues
- SignalR requires the server to run on HTTPS.
- Ensure the database is created (if you see "no such table: Flights" error, run migrations).
- Update connection strings in appsettings.json as needed.

## Contact
- Lead Developer: shira@example.com
- For questions or bug reports: open an Issue on GitHub or contact via email. 