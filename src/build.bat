@echo off
chcp 65001 >nul
echo ========================================
echo   CopyRelay C# 版本构建工具
echo ========================================
echo.

:: 检查 dotnet SDK
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [!] 未检测到 .NET SDK
    echo.
    echo 请安装 .NET 8 SDK:
    echo https://dotnet.microsoft.com/download/dotnet/8.0
    echo.
    echo 或运行以下命令安装:
    echo winget install Microsoft.DotNet.SDK.8
    echo.
    pause
    exit /b 1
)

echo [1/2] 正在编译...
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o publish

if %errorlevel% equ 0 (
    echo.
    echo [2/2] 编译成功！
    echo.

    :: 显示文件大小
    for %%A in (publish\CopyRelay.exe) do (
        set /a size=%%~zA / 1024
        echo 输出文件: publish\CopyRelay.exe
        echo 文件大小: %%~zA 字节
    )

    echo.
    echo ========================================
    echo   构建完成！
    echo ========================================
    explorer publish
) else (
    echo [X] 编译失败
)

pause
