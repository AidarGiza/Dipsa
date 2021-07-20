//Конфигурация и инициализация Google Play Services
*////
    using GooglePlayGames;
    using GooglePlayGames.BasicApi;
    using UnityEngine.SocialPlatforms;
	
    // Рекомендовано для откладки:
    PlayGamesPlatform.DebugLogEnabled = true;
    // Активировать Google Play Games Platform
    PlayGamesPlatform.Activate();
////*
//Логин игрока
*////
    using GooglePlayGames;
    using UnityEngine.SocialPlatforms;
    ...
    // Аутентификация игрока:
    Social.localUser.Authenticate((bool success) => {
        // Удачно или нет?
    });
////*
//Разблокирование достижения
*////
    using GooglePlayGames;
    using UnityEngine.SocialPlatforms;
    ...
    // Разблокировать достижение (ID "Cfjewijawiu_QA")
    Social.ReportProgress("Cfjewijawiu_QA", 100.0f, (bool success) => {
      // Удачно или нет?
    });
////*
//Результат в таблицу
*////
	using GooglePlayGames;
    using UnityEngine.SocialPlatforms;
    ...
    // Опубликовать счёт 12345 в таблицу ID "Cfji293fjsie_QA")
    Social.ReportScore(12345, "Cfji293fjsie_QA", (bool success) => {
        // Удачно или нет?
    });
////*
//Отображение списка достижений
*////
    using GooglePlayGames;
    using UnityEngine.SocialPlatforms;
    ...
    // Показать список достижений.
    Social.ShowAchievementsUI();
////*
//Отображение таблицы
*////
    using GooglePlayGames;
    using UnityEngine.SocialPlatforms;
    ...
    // Отображение таблицы.
    Social.ShowLeaderboardUI();
////*
//Выход из GPS
*////
    using GooglePlayGames;
    using UnityEngine.SocialPlatforms;
	...
    // Выйти
    PlayGamesPlatform.Instance.SignOut();
////*