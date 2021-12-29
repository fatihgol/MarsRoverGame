cd .\Satellite\MarsRoverGame.Satellite
start /d "." dotnet  run --urls=https://localhost:7601/
cd ..\..\Houston\MarsRoverGame.Houston
start /d "." dotnet run --urls=https://localhost:7600/
start "" "https://localhost:7600"