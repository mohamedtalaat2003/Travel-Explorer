# الـ Stage الأول: بيئة التشغيل (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# الـ Stage الثاني: بيئة البناء وتثبيت الحزم (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# نسخ ملفات الـ csproj للاستفادة من الـ Cache الخاص بـ Docker
COPY ["Travel Explorer/Travel Explorer.csproj", "Travel Explorer/"]
COPY ["Travel Explorer.Application/Travel Explorer.Application.csproj", "Travel Explorer.Application/"]
COPY ["Travel Explorer.Domain/Travel Explorer.Domain.csproj", "Travel Explorer.Domain/"]
COPY ["Travel Explorer.Infrastructure/Travel Explorer.Infrastructure.csproj", "Travel Explorer.Infrastructure/"]

# عمل Restore للحزم
RUN dotnet restore "Travel Explorer/Travel Explorer.csproj"

# نسخ باقي الكود وعمل Build
COPY . .
WORKDIR "/src/Travel Explorer"
RUN dotnet build "Travel Explorer.csproj" -c Release -o /app/build

# الـ Stage الثالث: تجهيز الملفات النهائية للنشر
FROM build AS publish 
RUN dotnet publish "Travel Explorer.csproj" -c Release -o /app/publish /p:UseAppHost=false

# الـ Stage الأخير: بناء الصورة النهائية للتشغيل فقط بدون الـ SDK
FROM base AS final 
WORKDIR /app
COPY --from=publish /app/publish .

# تشغيل الـ API مباشرة
ENTRYPOINT ["dotnet", "Travel Explorer.dll"]