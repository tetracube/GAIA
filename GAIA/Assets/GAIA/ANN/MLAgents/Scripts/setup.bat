@ for /f "delims=" %%i in ('python --version') do @ set pythonVersion=%%i
@ echo %pythonVersion% | find "Python 3.9.12" >nul
@ if errorlevel 1 (
    echo Python version must be 3.9.12
    set /p nul="Press any key to continue..."
    exit /b 0
)

@ for /f "delims=" %%i in ('pip --version') do @ set pipVersion=%%i
@ echo %pipVersion% | find "pip 23.2" >nul
@ if errorlevel 1 (
    echo Pip version must be 23.2
    set /p nul="Press any key to continue..."
    exit /b 0
)

python -m venv MLAEnvironment
call MLAEnvironment\Scripts\activate

pip install torch==2.0.1 torchvision torchaudio --index-url https://download.pytorch.org/whl/cu117
pip install mlagents==0.30.0
pip install mlagents-envs==0.30.0
pip install protobuf==3.20.3
pip install onnx==1.14.0

echo:
set /p nul="Installation completed successfully. Press any key to continue..."