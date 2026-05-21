using System;
using System.Collections.Generic;

namespace CameraPhotoSystem.Config
{
    public enum Language { CH, DE }

    public static class L
    {
        public static Language Current { get; set; } = Language.DE;

        private static readonly Dictionary<string, Dictionary<Language, string>> Dict = new Dictionary<string, Dictionary<Language, string>>
        {
            { "TabProduction", new Dictionary<Language, string> { { Language.CH, "一般生產" }, { Language.DE, "Produktion" } } },
            { "TabEngineering", new Dictionary<Language, string> { { Language.CH, "工程調機" }, { Language.DE, "Technik" } } },
            { "LblScan", new Dictionary<Language, string> { { Language.CH, "1. 請掃描標籤:" }, { Language.DE, "1. Etikett scannen:" } } },
            { "LblProgress", new Dictionary<Language, string> { { Language.CH, "2. 拍照進度:" }, { Language.DE, "2. Fotofortschritt:" } } },
            { "BtnCapture_Wait", new Dictionary<Language, string> { { Language.CH, "等待掃描..." }, { Language.DE, "Warten auf Scan..." } } },
            { "BtnCapture_Ready", new Dictionary<Language, string> { { Language.CH, "相機 1：拍攝第 1 張" }, { Language.DE, "Kamera 1: Foto 1 aufnehmen" } } },
            { "BtnCapture_Step", new Dictionary<Language, string> { { Language.CH, "相機 {0}：拍攝第 {1} 張" }, { Language.DE, "Kamera {0}: Foto {1} aufnehmen" } } },
            { "BtnQuery", new Dictionary<Language, string> { { Language.CH, "歷史記錄查詢" }, { Language.DE, "Historie abfragen" } } },
            { "GrpHardware", new Dictionary<Language, string> { { Language.CH, "相機硬體參數調整" }, { Language.DE, "Hardware-Parameter" } } },
            { "LblEngCamSelect", new Dictionary<Language, string> { { Language.CH, "0. 選擇欲查看的相機:" }, { Language.DE, "0. Kamera wählen:" } } },
            { "LblEngHardwareSelect", new Dictionary<Language, string> { { Language.CH, "1. 選擇硬體進行預覽:" }, { Language.DE, "1. Hardware wählen:" } } },
            { "LblEngAssign", new Dictionary<Language, string> { { Language.CH, "2. 確認影像後，指定給:" }, { Language.DE, "2. Position zuweisen:" } } },
            { "BtnHardware", new Dictionary<Language, string> { { Language.CH, "3. 開啟硬體設定" }, { Language.DE, "3. Hardware-Setup" } } },
            { "LblEngHint", new Dictionary<Language, string> { { Language.CH, "* 修改完成後請重新啟動程式以生效。" }, { Language.DE, "* Bitte starten Sie die App neu." } } },
            { "MsgPassword", new Dictionary<Language, string> { { Language.CH, "請輸入工程密碼:" }, { Language.DE, "Passwort eingeben:" } } },
            { "MsgWrongPwd", new Dictionary<Language, string> { { Language.CH, "密碼錯誤！" }, { Language.DE, "Falsches Passwort!" } } },
            { "LogStart", new Dictionary<Language, string> { { Language.CH, "系統啟動中..." }, { Language.DE, "System wird gestartet..." } } },
            { "LogDetected", new Dictionary<Language, string> { { Language.CH, "偵測完成：已啟動 {0} 支相機。" }, { Language.DE, "Erkennung abgeschlossen: {0} Kamera(s) aktiv." } } },
            { "LogFocusSet", new Dictionary<Language, string> { { Language.CH, "相機 2 焦距已自動設定為 31" }, { Language.DE, "Kamera 2 Fokus wurde auf 31 gesetzt." } } },
            { "LogSystemReady", new Dictionary<Language, string> { { Language.CH, "系統就緒，初始相機預覽已開啟。" }, { Language.DE, "System bereit, Kamera-Vorschau gestartet." } } },
            { "LogCapturing", new Dictionary<Language, string> { { Language.CH, "第 {0} 張拍照中 (相機 {1})..." }, { Language.DE, "Foto {0} wird aufgenommen (Kamera {1})..." } } },
            { "LogCaptureDone", new Dictionary<Language, string> { { Language.CH, "拍照完成，記錄已儲存。" }, { Language.DE, "Aufnahme abgeschlossen, Datensatz gespeichert." } } },
            { "LogSwitchCam", new Dictionary<Language, string> { { Language.CH, "自動切換至相機 {0} 預覽" }, { Language.DE, "Automatisch auf Kamera {0} gewechselt." } } },
            { "BtnLang", new Dictionary<Language, string> { { Language.CH, "切換語系 (Language):" }, { Language.DE, "Sprache wählen:" } } },
            { "WindowTitle", new Dictionary<Language, string> { { Language.CH, "相機拍照系統" }, { Language.DE, "Werk-Fotosystem" } } }
        };

        public static string T(string key)
        {
            if (Dict.ContainsKey(key) && Dict[key].ContainsKey(Current))
                return Dict[key][Current];
            return key;
        }
    }
}