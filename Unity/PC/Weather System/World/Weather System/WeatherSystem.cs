using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class WeatherSystem : MonoBehaviour
{
    [Header("World")]
    public World world;

    [Header("Sun/Moon")]
    public GameObject SunLight;
    public GameObject MoonLight;

    [Header("Sun/Moon Movement Points")]
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;

    [Header("Booleans")]
    public bool isDay = true;
    public bool ReachedTwo;
    public bool ReachedThree;
    public bool ReachedFour;

    [Header("Volume Profiles")]
    public Volume CurrentVolume;
    public VolumeProfile ExtraSunnyVolumeProfile;
    public VolumeProfile SunnyVolumeProfile;
    public VolumeProfile DuskVolumeProfile;
    public VolumeProfile NeutralVolumeProfile;
    public VolumeProfile FogVolumeProfile;
    public VolumeProfile ThunderVolumeProfile;
    public VolumeProfile NightVolumeProfile;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // SUN/MOON MOVEMENT //

        if (isDay)
        {
            SunMovement();
        }
        else
        {
            MoonMovement();
        }

        // INPUT //

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ExtraSunny();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Sunny();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Dusk();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Neutral();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Fog();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Thunder();
        }
    }

    // SUN/MOON MOVEMENT //

    void SunMovement()
    {
        // MOVE TO TWO //
        if (SunLight.transform.position == Two.transform.position && ReachedTwo != true)
        {
            ReachedTwo = true;
        }
        else if (ReachedTwo != true)
        {
            SunLight.transform.position = Vector3.MoveTowards(SunLight.transform.position, Two.transform.position, world.SunSpeed * Time.deltaTime);
        }

        if (ReachedTwo == true)
        {
            // MOVE TO THREE //
            if (SunLight.transform.position == Three.transform.position && ReachedThree != true)
            {
                ReachedThree = true;
            }
            else if (ReachedThree != true)
            {
                SunLight.transform.position = Vector3.MoveTowards(SunLight.transform.position, Three.transform.position, world.SunSpeed * Time.deltaTime);
            }
        }

        if (ReachedThree == true)
        {
            // MOVE TO FOUR //
            if (SunLight.transform.position == Four.transform.position && ReachedFour != true)
            {
                ReachedFour = true;
            }
            else if (ReachedFour != true)
            {
                SunLight.transform.position = Vector3.MoveTowards(SunLight.transform.position, Four.transform.position, world.SunSpeed * Time.deltaTime);
            }

            if (ReachedFour == true)
            {
                SetSun();
            }
        }
    }

    void MoonMovement()
    {
        // MOVE TO TWO //
        if (MoonLight.transform.position == Two.transform.position && ReachedTwo != true)
        {
            ReachedTwo = true;
        }
        else if (ReachedTwo != true)
        {
            MoonLight.transform.position = Vector3.MoveTowards(MoonLight.transform.position, Two.transform.position, world.MoonSpeed * Time.deltaTime);
        }

        if (ReachedTwo == true)
        {
            // MOVE TO THREE //
            if (MoonLight.transform.position == Three.transform.position && ReachedThree != true)
            {
                ReachedThree = true;
            }
            else if (ReachedThree != true)
            {
                MoonLight.transform.position = Vector3.MoveTowards(MoonLight.transform.position, Three.transform.position, world.MoonSpeed * Time.deltaTime);
            }
        }

        if (ReachedThree == true)
        {
            // MOVE TO FOUR //
            if (MoonLight.transform.position == Four.transform.position && ReachedFour != true)
            {
                ReachedFour = true;
            }
            else if (ReachedFour != true)
            {
                MoonLight.transform.position = Vector3.MoveTowards(MoonLight.transform.position, Four.transform.position, world.MoonSpeed * Time.deltaTime);
            }

            if (ReachedFour == true)
            {
                SetMoon();
            }
        }
    }

    // SET SUN/MOON //

    void SetSun()
    {
        SunLight.transform.position = One.transform.position;
        SunLight.gameObject.SetActive(false);
        MoonLight.gameObject.SetActive(true);
        ReachedTwo = false;
        ReachedThree = false;
        ReachedFour = false;
        isDay = false;
        Night();
    }

    void SetMoon()
    {
        MoonLight.transform.position = One.transform.position;
        SunLight.gameObject.SetActive(true);
        MoonLight.gameObject.SetActive(false);
        ReachedTwo = false;
        ReachedThree = false;
        ReachedFour = false;
        isDay = true;
        RandomWeather();
    }

    // WEATHER TYPE //

    void RandomWeather()
    {
        int rng = Random.Range(1, 7);
        switch (rng)
        {
            case 1:
                ExtraSunny();
                break;
            case 2:
                Thunder();
                break;
            case 3:
                Fog();
                break;
            case 4:
                Neutral();
                break;
            case 5:
                Dusk();
                break;
            case 6:
                Sunny();
                break;
        }
    }

    IEnumerator Fade(Volume current, VolumeProfile next, float FadeTime)
    {
        float startVolume = current.weight;
        while (current.weight > 0)
        {
            current.weight -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        Volume t = current;
        current.sharedProfile = next;

        while (t.weight < 1)
        {
            t.weight += startVolume * Time.deltaTime * FadeTime;
            yield return null;
        }


        if(t.weight > 1)
        {
            t.weight = 1;
        }

        yield return null;
    }



    void ExtraSunny()
    {
        StartCoroutine(Fade(CurrentVolume, ExtraSunnyVolumeProfile, 1f));

        // How to get volumeprofile settings example https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@10.0/manual/Volumes-API.html //
        /*
        if (!ExtraSunnyVolumeProfile.TryGet<Fog>(out var fog))
        {
            fog = ExtraSunnyVolumeProfile.Add<Fog>(false);
        }

        fog.enabled.overrideState = true;
        fog.enabled.value = true;
        */

    }
    void Sunny()
    {
        StartCoroutine(Fade(CurrentVolume, SunnyVolumeProfile, 1f));
    }

    void Dusk()
    {
        StartCoroutine(Fade(CurrentVolume, DuskVolumeProfile, 1f));
    }

    void Neutral()
    {
        StartCoroutine(Fade(CurrentVolume, NeutralVolumeProfile, 1f));
    }

    void Fog()
    {
        StartCoroutine(Fade(CurrentVolume, FogVolumeProfile, 1f));
    }

    void Thunder()
    {
        StartCoroutine(Fade(CurrentVolume, ThunderVolumeProfile, 1f));
    }

    void Night()
    {
        StartCoroutine(Fade(CurrentVolume, NightVolumeProfile, 1f));
    }
}
