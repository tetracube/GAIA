call MLAEnvironment\Scripts\activate

if "%~2"=="resume" (
    mlagents-learn "Assets\GAIA\ANN\MLAgents\EnvConfigs\env_config.yaml" --time-scale=1 --target-frame-rate=60 --initialize-from=%~1 --force
) else (
    mlagents-learn "Assets\GAIA\ANN\MLAgents\EnvConfigs\env_config.yaml" --time-scale=1 --target-frame-rate=60 --run-id=%~1 --force
)