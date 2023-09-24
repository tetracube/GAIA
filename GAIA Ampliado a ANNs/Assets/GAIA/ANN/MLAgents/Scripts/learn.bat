call MLAEnvironment\Scripts\activate

if "%~2"=="resume" (
    mlagents-learn "Assets\GAIA\ANN\MLAgents\EnvConfigs\env_config.yaml" --initialize-from=%~1 --force
) else (
    mlagents-learn "Assets\GAIA\ANN\MLAgents\EnvConfigs\env_config.yaml" --run-id=%~1 --force
)
