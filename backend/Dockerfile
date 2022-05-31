FROM mcr.microsoft.com/dotnet/sdk:5.0.408-bullseye-slim-amd64
ADD . /backend
CMD [ "dotnet", "run","--project","/backend/backend.csproj","migrate=true"]