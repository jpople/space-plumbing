[System.Serializable]
public class ModuleConfig // at some point it may behoove us to have this implement IEquatable so it works with lists, dictionaries etc.-- probably not necessary yet though
{
    public bool isPhaseModulationMono = true;
    public bool isKhuranaMatrixNormal = true;
    public enum FiltrationSetting {
        Low, // 0
        Medium, // 1
        High, // 2
        Variant // 3
    };
    public FiltrationSetting proxyFiltration = FiltrationSetting.Low;
    public int stabilizerAmplitude = 0;
    public int catalystUpperBound = 22;
}

public struct RequiredConfig {
    public bool requiresMonoModulation;
    public bool requiresNormalMatrix;
    public ModuleConfig.FiltrationSetting requiredFiltration;
    public int stabilizerMin;
    public int stabilizerMax;
    public int catalystMin;
    public int catalystMax;

    public RequiredConfig(bool requiresMonoModulation, bool requiresNormalMatrix, ModuleConfig.FiltrationSetting requiredFiltration, int stabilizerMin, int stabilizerMax, int catalystMin, int catalystMax)
    {
        this.requiresMonoModulation = requiresMonoModulation;
        this.requiresNormalMatrix = requiresNormalMatrix;
        this.requiredFiltration = requiredFiltration;
        this.stabilizerMin = stabilizerMin;
        this.stabilizerMax = stabilizerMax;
        this.catalystMin = catalystMin;
        this.catalystMax = catalystMax;
    }
}