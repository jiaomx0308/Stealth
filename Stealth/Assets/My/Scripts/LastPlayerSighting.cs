using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LastPlayerSighting : MonoBehaviour
{
    public Vector3 position = new Vector3(1000f, 1000f, 1000f); //表示玩家最后一次被发现的位置，如果没有被发现，就设置为默认值
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
    public float lightHighIntensity = 0.25f;  //主灯光的亮度范围
    public float lightLowIntensity = 0f;
    public float lightFadeSpeed = 7f;
    public float musicFadeSpeed = 1f;  //音乐变化的fade速率
    public bool isPlayerFound = false;

    public static LastPlayerSighting Instance { get; private set; }

    private AlarmLight alarmLightScript;
    private Light mainLight;          //主灯光
    private AudioSource mainMusic;  //主音乐和panic时播放的音乐
    private AudioSource panicMusic;
    private AudioSource[] sirens;  //报警音乐
    private const float muteVolume = 0f; //音乐的变化范围
    private const float normalVolume = 0.8f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        File.WriteAllText("jiaomx.log", "************");

        alarmLightScript = GameObject.FindGameObjectWithTag(Tags.ALARM_LIGHT).GetComponent<AlarmLight>();
        mainLight = GameObject.FindGameObjectWithTag(Tags.MAIN_LIGHT).GetComponent<Light>();
        mainMusic = this.GetComponent<AudioSource>();
        panicMusic = this.transform.Find("Secondary_music").GetComponent<AudioSource>();
        //sirens = new AudioSource[];
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerFound = (position != resetPosition);

        //当玩家被发现时，调低主灯光，打开报警灯，淡出主音乐，淡入panic音乐， 但玩家脱离危险后恢复；
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, isPlayerFound ? lightLowIntensity : lightHighIntensity, lightFadeSpeed * Time.deltaTime);
        alarmLightScript.alarmOn = isPlayerFound;
        mainMusic.volume = Mathf.Lerp(mainMusic.volume, isPlayerFound ? muteVolume : normalVolume, musicFadeSpeed);
        panicMusic.volume = Mathf.Lerp(panicMusic.volume, isPlayerFound ? normalVolume : muteVolume, musicFadeSpeed);
    }
}
