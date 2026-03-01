# Giai đoạn build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file .csproj và restore các dependencies
COPY ["ClinicManagement.csproj", "./"]
RUN dotnet restore "ClinicManagement.csproj"

# Copy toàn bộ source code và build ứng dụng
COPY . .

# Build và publish ứng dụng ra thư mục /app/publish
RUN dotnet publish "ClinicManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Giai đoạn runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Chuyển các file đã build từ giai đoạn build sang
COPY --from=build /app/publish .

# Thiết lập biến môi trường để chạy app trên cổng 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "ClinicManagement.dll"]
