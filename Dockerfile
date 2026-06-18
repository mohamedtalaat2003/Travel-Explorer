FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base 

WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENV ConnectionStrings__DefaultConnection="Host=ep-damp-thunder-amr0o6pj-pooler.c-5.us-east-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_QCusy7ZTM8Ua;SSL Mode=Require;Trust Server Certificate=true;"

# نستخدم صورة الـ SDK كاملة لأنها تحتوي على أدوات البناء
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# في الخطوات التالية نقوم بنسخ ملفات الـ csproj فقط لكل المشاريع
# لماذا؟ لكي يستفيد Docker من ميزة الـ Cache ولا يحمل الحزم (Packages) في كل مرة نعدل فيها الكود
COPY ["Travel Explorer/Travel Explorer.csproj", "Travel Explorer/"]
COPY ["Travel Explorer.Application/Travel Explorer.Application.csproj", "Travel Explorer.Application/"]
COPY ["Travel Explorer.Domain/Travel Explorer.Domain.csproj", "Travel Explorer.Domain/"]
COPY ["Travel Explorer.Infrastructure/Travel Explorer.Infrastructure.csproj", "Travel Explorer.Infrastructure/"]

# تحميل وتثبيت جميع الحزم (Nuget packages) المطلوبة للمشروع
RUN dotnet restore "Travel Explorer/Travel Explorer.csproj"

# الآن ننسخ باقي ملفات الكود كلها من جهازك إلى داخل الحاوية
COPY . .

# ندخل إلى مجلد المشروع الرئيسي
WORKDIR "/src/Travel Explorer"

# نقوم ببناء المشروع في وضع الـ Release
RUN dotnet build "Travel Explorer.csproj" -c Release -o /app/build

FROM build AS publish 

RUN dotnet publish "Travel Explorer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final 

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Travel Explorer.dll"]