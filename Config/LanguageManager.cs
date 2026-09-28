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
            { "WindowTitle", new Dictionary<Language, string> { { Language.CH, "相機拍照系統 V1.1.0" }, { Language.DE, "Werk-Fotosystem V1.1.0" } } },
            { "BtnSettings", new Dictionary<Language, string> { { Language.CH, "系統參數設定" }, { Language.DE, "Systemeinstellungen" } } },
            { "SettingTitle", new Dictionary<Language, string> { { Language.CH, "系統參數設定" }, { Language.DE, "Systemeinstellungen" } } },
            { "LblLineName", new Dictionary<Language, string> { { Language.CH, "產線名稱 (Line Name):" }, { Language.DE, "Linienname (Line Name):" } } },
            { "LblPhotoRoot", new Dictionary<Language, string> { { Language.CH, "照片存放目錄 (Photo Root):" }, { Language.DE, "Fotopfad (Photo Root):" } } },
            { "LblMaxCount", new Dictionary<Language, string> { { Language.CH, "最大拍照張數 (Max Photos):" }, { Language.DE, "Max. Fotos pro Zyklus:" } } },
            { "LblDesiredWidth", new Dictionary<Language, string> { { Language.CH, "相機目標寬度解析度:" }, { Language.DE, "Ziel-Breitenauflösung:" } } },
            { "BtnBrowse", new Dictionary<Language, string> { { Language.CH, "瀏覽..." }, { Language.DE, "Durchsuchen..." } } },
            { "BtnSave", new Dictionary<Language, string> { { Language.CH, "儲存設定" }, { Language.DE, "Speichern" } } },
            { "BtnCancel", new Dictionary<Language, string> { { Language.CH, "取消" }, { Language.DE, "Abbrechen" } } },
            { "MsgSaveSuccess", new Dictionary<Language, string> { { Language.CH, "設定已成功儲存！" }, { Language.DE, "Einstellungen erfolgreich gespeichert!" } } },
            { "LblUploadPath", new Dictionary<Language, string> { { Language.CH, "上傳目錄 (Upload Path):" }, { Language.DE, "Upload-Pfad:" } } },
            { "LogCsvExportSuccess", new Dictionary<Language, string> { { Language.CH, "CSV 匯出成功並已同步上傳: {0}" }, { Language.DE, "CSV exportiert und hochgeladen: {0}" } } },
            { "LogCsvUploadRetry", new Dictionary<Language, string> { { Language.CH, "【警告】CSV 上傳失敗，正在進行第 {0}/3 次重試..." }, { Language.DE, "【Warnung】Upload fehlgeschlagen, Wiederholung {0}/3..." } } },
            { "MsgCsvUploadFailed", new Dictionary<Language, string> { { Language.CH, "【警告】條碼 {0} 的 CSV 上傳至 [{1}] 失敗 (已重試 3 次)！\n請檢查網路連線或上傳路徑。\n\n本地 CSV 已安全保存在:\n{2}" }, { Language.DE, "【Warnung】CSV-Upload für Code {0} fehlgeschlagen (3 Versuche)!\nBitte Netzwerk oder Pfad prüfen.\n\nLokale Datei gespeichert unter:\n{2}" } } },
            { "BtnTest", new Dictionary<Language, string> { { Language.CH, "測試" }, { Language.DE, "Test" } } },
            { "MsgTestUploadSuccess", new Dictionary<Language, string> { { Language.CH, "測試 CSV 產生與上傳成功！\n\n已成功上傳至：\n{0}" }, { Language.DE, "Test-CSV erfolgreich erstellt und hochgeladen!\n\nErfolgreich hochgeladen nach:\n{0}" } } },
            { "MsgTestUploadFailed", new Dictionary<Language, string> { { Language.CH, "測試 CSV 上傳失敗！\n\n錯誤原因：\n{0}" }, { Language.DE, "Test-CSV Upload fehlgeschlagen!\n\nFehlerursache:\n{0}" } } },
            { "LogScannerStarted", new Dictionary<Language, string> { { Language.CH, "掃描器已啟動於 {0} (鮑率: {1})" }, { Language.DE, "Scanner gestartet an {0} (Baudrate: {1})" } } },
            { "LogScannerConnectFailed", new Dictionary<Language, string> { { Language.CH, "掃描器連線失敗: {0}" }, { Language.DE, "Scanner-Verbindung fehlgeschlagen: {0}" } } },
            { "LogDuplicateLocked", new Dictionary<Language, string> { { Language.CH, "【警告】連續條碼重複，系統已鎖定。" }, { Language.DE, "【Warnung】Fortlaufend doppelter Code, System gesperrt." } } },
            { "LogScanSuccess", new Dictionary<Language, string> { { Language.CH, "掃描成功: {0}" }, { Language.DE, "Scan erfolgreich: {0}" } } },
            { "LogRestoringProduction", new Dictionary<Language, string> { { Language.CH, "正在從工程模式恢復生產連線..." }, { Language.DE, "Wiederherstellung der Produktionsverbindung..." } } },
            { "LogSettingsUpdated", new Dictionary<Language, string> { { Language.CH, "系統設定已更新並套用。" }, { Language.DE, "Systemeinstellungen aktualisiert und übernommen." } } },
            { "MsgCamApplied", new Dictionary<Language, string> { { Language.CH, "相機設定已生效！" }, { Language.DE, "Kamera-Einstellungen übernommen!" } } },
            { "MsgScannerApplied", new Dictionary<Language, string> { { Language.CH, "掃描器設定已儲存並重啟" }, { Language.DE, "Scanner-Einstellungen gespeichert und neu gestartet." } } },
            { "LogErrorPrefix", new Dictionary<Language, string> { { Language.CH, "【錯誤】" }, { Language.DE, "【Fehler】" } } },
            { "ErrPortNotExist", new Dictionary<Language, string> { { Language.CH, "通訊埠 '{0}' 不存在。" }, { Language.DE, "Der Port '{0}' existiert nicht." } } },
            { "ErrPortAccessDenied", new Dictionary<Language, string> { { Language.CH, "通訊埠 '{0}' 存取被拒 (可能已被其他程式佔用)。" }, { Language.DE, "Zugriff auf Port '{0}' verweigert (bereits belegt)." } } }
        };

        public static string T(string key)
        {
            if (Dict.ContainsKey(key) && Dict[key].ContainsKey(Current))
                return Dict[key][Current];
            return key;
        }
    }
}