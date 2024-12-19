using Expedition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environment = Expedition.Models.Environment;

namespace Expedition
{
    public class InitializeGame
    {
        public GameModel CreateExpeditionGame(string playerName)
        {
            _model = new GameModel();

            _model.Player.Name = playerName;

            _model.SetItems(CreateGameItems());
            _model.SetNPCS(CreateGameNPC());
            _model.SetEnviromnets(CreateGameEnvironments());
            _model.SetTransfers(CreateGameExits());
            _model.SetQuests(CreateGameQuests());
            _model.SetCurrentQuest(0);
            _model.SetCurrentEnvironment(0);

            return _model;
        }

        GameModel _model;
        List<IInventoryItem> _items;
        List<NPC> _characters;
        List<Environment> _environments;
        List<ExitsFromEnvironment> _exits;
        List<Quest> _quests;


        private List<IInventoryItem> CreateGameItems()
        {
            _items = new List<IInventoryItem>();

            //Еда
            _items.Add(new Food()
            {
                Name = "Маленькая булочка",
                Description = "Восстанавливает мало сытости(15 ед.), зато легко помещается в карман.",
                Saturation = 15,
                MaximumCount = 4,
                SecondName = "Маленькую булочку"
            });
            _items.Add(new Food()
            {
                Name = "Сытный пирожок",
                Description = "Восстанавливает прилично сытости(30 ед.), но для носки не помешает рюкзак.",
                Saturation = 30,
                MaximumCount = 2
            });

            //Лечение
            _items.Add(new HealthKit()
            {
                Name = "Бинт",
                Description = "Восстанавливает немного здоровья(4 ед.), незаменимая вещь в путешествии.",
                RecoverableHealth = 4,
                MaximumCount = 5
            });
            _items.Add(new HealthKit()
            {
                Name = "Аптечка",
                Description = "Восстанавливает половину здоровья(10 ед.), самая важная вешь в приключениях, никто же не хочет тут умереть.",
                RecoverableHealth = 10,
                MaximumCount = 2,
                SecondName = "Аптечку"
            });

            //Квестовые предметы
            _items.Add(new QuestItem()
            {
                Name = "Рюкзак",
                Description = "Это ваш рюкзак, благодаря нему вы можете таскать с собой веши.",
                Action = "Теперь вам доступен инвентарь.",
                MaximumCount = 1, 
                IsVisible = false
            });
            _items.Add(new QuestItem()
            {
                Name = "Ноутбук",
                Description = "Ваш рабочий ноутбук, вы любите сидеть за ним после учебы. Учеба и развлечения в одном месте, что может быть лучше. Похоже на почте новое <уведомление>",
                Action = "Кажется, вам пришло новое уведомление.",
                MaximumCount = 0
            });
            _items.Add(new QuestItem()
            {
                Name = "Уведомление",
                Description = $"Уведомление из университета. Стоит прочитать, что там написано. Уведомление:\nУважаемый {_model.Player.Name}, Мы рады сообщить вам о возможности присоединиться к нашей экспедиции по исследованию неизведанных глубин океана на планете 4546B. Ваша страсть к океанологии и ваш академический успех сделали вас идеальным кандидатом для участия в этой экспедиции. Мы ищем смелых и любознательных людей, готовых столкнуться с неизведанным и раскрыть тайны подводного мира. Экспедиция начнется через две недели. Вам предстоит работать в команде с учеными и инженерами, которые разделяют вашу любовь к океану. Это шанс не только проверить свои силы, но и внести свой вклад в научные исследования. С нетерпением ждем вас через две недели в пункте отправки университета!\n",
                Action = "Это уведомление мечты, лучше просто и быть не может.",
                MaximumCount = 0
            });


            _items.Add(new QuestItem()
            {
                Name = "Оборудование",
                Description = "Оборудование необходимое для экспедиции.",
                Action = "С этим оборудованием можно отправляться в экспедицию.",
                MaximumCount = 1
            });

            _items.Add(new QuestItem()
            {
                Name = "Оранжевый коралл",
                Description = "Оранжевый коралл полукруглой формы. Неплохой сувенир.",
                Action = "Сувенир.",
                MaximumCount = 1
            });

            _items.Add(new QuestItem()
            {
                Name = "Плод зеленых водорослей",
                Description = "Большой желтый плод, по форме напоминает каплю воды. Светится.",
                Action = "Это, можно сказать, переносная лампочка.",
                MaximumCount = 1
            });

            _items.Add(new QuestItem()
            {
                Name = "Непонятное устройство",
                Description = "Небольшое устройство, по размерам можно сравнить с буханкой хлеба, по форме же это прямоугольник с зелеными светящимися полосами по периметру.",
                Action = "Требуется изучение",
                MaximumCount = 1
            });

            return _items;
        }

        private List<NPC> CreateGameNPC()
        {
            _characters = new List<NPC>();

            var drSmith = new NPC("Др. Смит");
            drSmith.AddPhrase("Я занимаюсь анализом данных о морской жизни.", "Институт");
            drSmith.AddPhrase("Рад, что тебе получилось попасть сюда. Это ценный опыт для тебя. Надеюсь тебе понравиться экспедиция.", "Спальня");

            var Jane = new NPC("Джейн");
            Jane.AddPhrase("Я отвечаю за техническое обеспечение нашей команды.", "Институт");
            Jane.AddPhrase("Интересный кабинет, правда? Благодаря современным технологиям врачи нам уже не нужны, в подобных кабинетах все полностью автоматизировано. Обычные медикаменты тоже есть, но ими обучают пользоваться в институте, надеюсь ты не пропускал эти занятия.", "Мед. кабинет");

            var Alex = new NPC("Алекс");
            Alex.AddPhrase("Я изучаю экосистемы океана и помогаю команде с анализом.", "Институт");
            Alex.AddPhrase("Тоже ищешь поесть?", "Кухня");


            _characters.Add(drSmith);
            _characters.Add(Jane);
            _characters.Add(Alex);

            return _characters;
        }

        private List<Environment> CreateGameEnvironments()
        {
            _environments = new List<Environment>();

            var homeRoom = new Environment("Твоя комната", "Главный герой живет в небольшой, но уютной однокомнатной квартире, где царит атмосфера спокойствия и уюта. В комнате стоит удобная кровать, рядом с которой находится письменный стол с <ноутбуком>, на котором герой работает над своими проектами. На стенах висят фотографии из путешествий и картины с изображениями подводного мира, отражающие его увлечение океанологией. В кухонной зоне расположены необходимые бытовые приборы, а на столе — несколько книг и чашка с горячим чаем. Большие окна пропускают много света и открывают вид на город, создавая ощущение простора и тепла.");
            homeRoom.AddItem(GetItem("Рюкзак"));
            homeRoom.AddItem(GetItem("Ноутбук"));
            homeRoom.AddItem(GetItem("Уведомление"));
            _environments.Add(homeRoom);

            var institute = new Environment("Институт", "Институт, где собираются участники экспедиции, представляет собой современное здание с большими стеклянными окнами, через которые проникает яркий солнечный свет. Внутри царит атмосфера активности и ожидания: студенты и ученые обсуждают свои исследования, а в воздухе витает запах свежезаваренного кофе. Помещение для сбора участников — просторный конференц-зал с удобными креслами и большим экраном для презентаций.На стенах висят карты океанских глубин и фотографии прошлых экспедиций, вдохновляющие на новые открытия. Участники собираются за длинным столом, где разложены документы и оборудование для предстоящего погружения. Здесь царит дух сотрудничества и энтузиазма, ведь каждый из них готов внести свой вклад в изучение загадок подводного мира.");
            institute.AddItem(GetItem("Оборудование"));
            institute.AddCharacter(GetCharacter("Др. Смит"));
            institute.AddCharacter(GetCharacter("Джейн"));
            institute.AddCharacter(GetCharacter("Алекс"));
            _environments.Add(institute);

            var submarine = new Environment("Основной шлюз", "Субмарина, на борту которой участники экспедиции отправляются исследовать глубины океана, представляет собой высокотехнологичное подводное судно, оснащенное всем необходимым для комфортного и безопасного погружения. Внутри субмарины царит атмосфера научного исследования: стены обшиты прочным металлом, а большие иллюминаторы позволяют наблюдать за подводным миром. Основное помещение — это просторный салон с удобными креслами и современным оборудованием для мониторинга состояния окружающей среды.Здесь находятся консоли управления, экраны с данными о глубине и температуре воды, а также карты исследуемых районов.Кухонный уголок предлагает простые, но питательные блюда, которые готовятся на борту. В отдельной каюте участники могут отдохнуть и подготовиться к предстоящим погружениям. Субмарина оборудована всем необходимым для длительных исследований, создавая идеальные условия для изучения загадок океанских глубин.", secondName: "субмарину");
            _environments.Add(submarine);

            var bedroom = new Environment("Спальня", "Это спальня, здесь отдыхают участники экспедиции", "спальню");
            bedroom.AddCharacter(GetCharacter("Др. Смит"));
            _environments.Add(bedroom);


            var kitchen = new Environment("Кухня", "Это кухня, здесь можно запастись едой.", "кухню");
            for(int i = 0; i < 500; i++)
            {
                kitchen.AddItem(GetItem("Маленькая булочка"));
                kitchen.AddItem(GetItem("Сытный пирожок"));
            }
            kitchen.AddCharacter(GetCharacter("Алекс"));
            _environments.Add(kitchen);

            var healthRoom = new Environment("Мед. кабинет", "Это мед. кабинет, здесь можно запастись медикаментами");
            for (int i = 0; i < 500; i++)
            {
                healthRoom.AddItem(GetItem("Бинт"));
                healthRoom.AddItem(GetItem("Аптечка"));
            }
            healthRoom.AddCharacter(GetCharacter("Джейн"));
            _environments.Add(healthRoom);

            var safeEnv = new Environment("Безопасные отмели", "Это безопасные отмели, место вашей остановки и основная зона в данном океане.");
            safeEnv.AddItem(GetItem("Оранжевый коралл"));
            safeEnv.SetDangerous(2);
            _environments.Add(safeEnv);
            
            var seaweedZone = new Environment("Зона водорослей", "Ближайшая к безопансым отмелям зона, она заполнена зелеными водорослями с желтыми светящимеся плодами.", "зону водорослей");
            seaweedZone.AddItem(GetItem("Плод зеленых водорослей"));
            seaweedZone.SetDangerous(2);
            _environments.Add(seaweedZone);

            var redDesert = new Environment("Красные пески", "Зона заполненная песками, получила свое нозвание из-за красных растений растущих из песка, в следствие чего вся зона кажется красной. По зоне также растилается множество гор, что делает ее трудной к передвижению на крупном транспорте.");
            redDesert.SetDangerous(2);
            _environments.Add(redDesert);

            var oldHouse = new Environment("Древний храм", "Древний подводных храм. Судя по всему его построили прошлые обитатели планеты, они были весьма разумны, чтобы построить такое. Перед вам открывается большой холл и вы видите развилку из двух коридоров.");
            oldHouse.SetDangerous(2);
            _environments.Add(oldHouse);
            
            var leftCorridor = new Environment("Левая комната", "Вы видите перед собой большую комнату. По периметру стен стоят каменные плиты и небольшие коробки из того же камня. Похоже это кровати с тумбочками, отсюда можно сделать вывод, что это общежитие, но, правда, больше походит на военный барак.", "левую комнату");
            leftCorridor.SetDangerous(2);
            _environments.Add(leftCorridor);

            var rightCorridor = new Environment("Правый коридор", "Длинный коридор с тусклым зеденым свеченим из под потолка, в конце вы видите переход в другую комнату, но из-за света не можете разглядеть что там.");
            rightCorridor.SetDangerous(2);
            _environments.Add(rightCorridor);

            var strangeRoom = new Environment("Странная комната", "Вы видите перед собой непонятную комнату, размера она примерно 3 метра в каждом параметре, зеленый свет из под потолка здесь уже достаточно ярко освещает комнату, в центре стоит подобите стола и на нем дежит <непонятное устройство>", "странную комнату");
            strangeRoom.AddItem(GetItem("Непонятное устройство"));
            strangeRoom.SetDangerous(2);
            _environments.Add(strangeRoom);

            return _environments;
        }

        private IInventoryItem GetItem(string itemName)
        {
            return _items.Where(x => x.Name == itemName).FirstOrDefault();
        }
        private NPC GetCharacter(string characterName)
        {
            return _characters.Where(x => x.Name == characterName).FirstOrDefault();
        }

        private List<ExitsFromEnvironment> CreateGameExits()
        {
            _exits = new List<ExitsFromEnvironment>();

            var homeRoomExits = new ExitsFromEnvironment("Твоя комната");
            homeRoomExits.AddExit(GetEnvironment("Институт"));

            _exits.Add(homeRoomExits);

            var institutExits = new ExitsFromEnvironment("Институт");
            institutExits.AddExit(GetEnvironment("Основной шлюз"));
            _exits.Add(institutExits);

            var submarineExits = new ExitsFromEnvironment("Основной шлюз");
            submarineExits.AddExit(GetEnvironment("Спальня"));
            submarineExits.AddExit(GetEnvironment("Кухня"));
            submarineExits.AddExit(GetEnvironment("Мед. кабинет"));
            submarineExits.AddExit(GetEnvironment("Безопасные отмели"));
            _exits.Add(submarineExits);

            var bedroomExits = new ExitsFromEnvironment("Спальня");
            bedroomExits.AddExit(GetEnvironment("Основной шлюз"));
            _exits.Add(bedroomExits);

            var kitchenExits = new ExitsFromEnvironment("Кухня");
            kitchenExits.AddExit(GetEnvironment("Основной шлюз"));
            _exits.Add(kitchenExits);

            var healthRoomExit = new ExitsFromEnvironment("Мед. кабинет");
            healthRoomExit.AddExit(GetEnvironment("Основной шлюз"));
            _exits.Add(healthRoomExit);

            var safeZoneExits = new ExitsFromEnvironment("Безопасные отмели");
            safeZoneExits.AddExit(GetEnvironment("Основной шлюз"));
            safeZoneExits.AddExit(GetEnvironment("Зона водорослей"));
            safeZoneExits.AddExit(GetEnvironment("Красные пески"));
            _exits.Add(safeZoneExits);

            var seaweedZoneExits = new ExitsFromEnvironment("Зона водорослей");
            seaweedZoneExits.AddExit(GetEnvironment("Безопасные отмели"));
            _exits.Add(seaweedZoneExits);

            var redDesertExits = new ExitsFromEnvironment("Красные пески");
            redDesertExits.AddExit(GetEnvironment("Безопасные отмели"));
            redDesertExits.AddExit(GetEnvironment("Древний храм"));
            _exits.Add(redDesertExits);

            var oldHouseExits = new ExitsFromEnvironment("Древний храм");
            oldHouseExits.AddExit(GetEnvironment("Красные пески"));
            oldHouseExits.AddExit(GetEnvironment("Левая комната"));
            oldHouseExits.AddExit(GetEnvironment("Правый коридор"));
            _exits.Add(oldHouseExits);

            var leftRoomExits = new ExitsFromEnvironment("Левая комната");
            leftRoomExits.AddExit(GetEnvironment("Древний храм"));
            _exits.Add(leftRoomExits);

            var rightCorridorExits = new ExitsFromEnvironment("Правый коридор");
            rightCorridorExits.AddExit(GetEnvironment("Древний храм"));
            rightCorridorExits.AddExit(GetEnvironment("Странная комната"));
            _exits.Add(rightCorridorExits);

            var strangeRoomExits = new ExitsFromEnvironment("Странная комната");
            strangeRoomExits.AddExit(GetEnvironment("Правый коридор"));
            _exits.Add(strangeRoomExits);

            return _exits;
        }

        private Environment GetEnvironment(string environmentName)
        {
            return _environments.Where(x => x.Name == environmentName).FirstOrDefault();
        }

        private List<Quest> CreateGameQuests()
        {
            _quests = new List<Quest>();

            var firstQuest = new Quest("Прибытие домой", "После долгого учебного дня вы возвращаетесь домой, уставший, но довольный. Как студент-исследователь в области океанологии, вы провели часы на лекциях и лабораторных занятиях, обсуждая с однокурсниками последние открытия в области подводных исследований.");

            var firstQuestSteps = new List<QuestStep>()
            {
                new QuestStep(new List<string>() { "осмотреться" }, "По прыбитию домой, не мешало бы осмотреться. С таким количеством учебы можно и забыть как родной дом выглядит. (Вы можете ввести команду \"осмотреться\" - она позволяет увидеть описание окружения.)", isMovement: false),
                new QuestStep(new List<string>() { "посмотреть на ноутбук", "посмотреть на уведомление" }, "Целый день без <ноутбука>, стоит проверить что там, да как.(Команда \"посмотреть на\" позволяет увидеть описание объекта; интерактивные объекты выделяются фигурными скобками: <объект>)", isMovement: false),
                new QuestStep(new List<string>() { "взять рюкзак" }, "Надеюсь время быстро пролетит. \n\nСпустя две недели...\n\nУже прошли две недели, пора собираться на выход. Нужно взять <рюкзак>.(Команада \"взять\" позволяет брать предметы в руки)", isMovement: false),
                new QuestStep(new List<string>() { "перейти в институт" }, "Вещи собраны, пора идти в <институт>.(Для перемешения используйте команду \"перейти в\")", new List<IInventoryItem>() { GetItem("Рюкзак")})
            };

            firstQuest.SetSteps(firstQuestSteps);

            var neccessaryFirstSteps = new List<QuestStep>()
            {
                firstQuestSteps[2],
                firstQuestSteps[3]
            };
            firstQuest.AddEnding("Ежедневыне поездки на общественном транспорте ваше любимое занятие по утрам.", neccessaryFirstSteps);

            _quests.Add(firstQuest);


            var instituteQuest = new Quest("Сбор в институте", "Небольшая поедка на общественном транспорте и вот вы уже в институте, место уже столь знакомое, но каждый раз вы любите осматриваеть его и наслаждаетесь местной красотой, сделаете ли вы это вновь?", items: new List<IInventoryItem>() { GetItem("Оборудование") });

            var instutueQuestSteps = new List<QuestStep>()
            {
                new QuestStep(new List<string>() { "исследовать" }, "Стоит понять, что я могу здесь сделать.(Команда \"исследовать\" покажет все предметы взаимодействия в округе)", isMovement: false),
                new QuestStep(new List<string>() { "поговорить с др. смит", "поговорить с джейн", "поговорить с алекс" }, "Не помешало бы познакомиться со всеми участниками экспедиции.(Команда \"поговорить с\" позволит поговорить с выбранным персонажем)", isMovement: false),
                new QuestStep(new List<string>() { "взять оборудование" }, "Теперь вы со всеми знакомы, необходимо взять <оборудование> для экспедиции.", isMovement: false),
                new QuestStep(new List<string>() { "перейти в субмарину" }, "С этим оборудованием можно спокойно садиться в <субмарину> и готовиться к эскпедиции")
            };

            instituteQuest.SetSteps(instutueQuestSteps);

            var neccessaryInstituteSteps = new List<QuestStep>()
            {
                instutueQuestSteps[1],
                instutueQuestSteps[2]
            };
            instituteQuest.AddEnding("Вы отправились в экспедицию", neccessaryInstituteSteps);

            _quests.Add(instituteQuest);

            var submarineQuest = new Quest("Осмотр субмарины", "Ваше приключение начинается и теперь вы остановились посреди океана, чтобы исследовать окрестности.");

            var submarineQuestSteps = new List<QuestStep>()
            {
                new QuestStep(new List<string>() { "перейти в спальню", "перейти в кухню", "перейти в мед. кабинет" }, "Субмарина удивительное средство передвижения, обустроенное для полного удобства при экспедиции. Стоит пройтись посмотреть все комнаты.(Чтобы узнать все переходы с локации можно воспользоваться командой \"выходы\")"),
                new QuestStep(new List<string>() { "взять маленькую булочку", "взять сытный пирожок", "взять бинт", "взять аптечку" }, "Перед исследованием ближайшей месности стоит запастись едой и медикаментами."),
                new QuestStep(new List<string>() { "перейти в безопасные отмели" }, "Теперь вы полностью снаряжены, можно отправляться на исследование местности. (Чтобы использовать собранные предметы используйте команду \"использовать\")")
            };

            submarineQuest.SetSteps(submarineQuestSteps);

            var neccessarySubmarineSteps = new List<QuestStep>()
            {
                submarineQuestSteps[0],
                submarineQuestSteps[1]
            };

            submarineQuest.AddEnding("Время опробовать себя в исследовании местности.", neccessarySubmarineSteps);

            _quests.Add(submarineQuest);

            var explorationQuest = new Quest("Ислледование местности", "Вы впервые спустились в воду для исследования, вам переполняют чувства. Давайте же узнаем, что скрывают эти воды.");

            var explorationQuestSteps = new List<QuestStep>()
            {
                new QuestStep(new List<string>() { "перейти в красные пески" }, "Стоит осмотреть близлежайшую местность, может получится найти что-то интересное."),
                new QuestStep(new List<string>() { "перейти в древний храм" }, $"Вам на КПК пришло сообщение: {_model.Player.Name}, послушай, это Др. Смит, неподалеку от тебя обнаружили странную активность, что-то похожее на древний храм. Раз ты ближе всех, посмотри, пожалуйста, что там происходит."),
            };

            explorationQuest.SetSteps(explorationQuestSteps);

            var neccessaryExplorationSteps = new List<QuestStep>()
            {
                explorationQuestSteps[1]
            };

            explorationQuest.AddEnding("Вы попали в древний храм, нужно исслеовать его, чтобы разобраться в ситуации.", neccessaryExplorationSteps);

            _quests.Add(explorationQuest);

            var endQuest = new Quest("Осмотреть древний храм", "По  просьбе Др. Смита вы вошли в древний храм. Найдите источник странного сигнала и выясните что это.");

            var endQuestSteps = new List<QuestStep>()
            {
                new QuestStep(new List<string>() { "перейти в правый коридор" }, "Стоит осмотреть здесь все."),
                new QuestStep(new List<string>() { "перейти в странную комнату" }, "Впереди видно комнату, ее обязательно следует проверить."),
                new QuestStep(new List<string>() { "посмотреть на непонятное устройство" }, "Стоит проверить это устройство."),
                new QuestStep(new List<string>() { "" }, "Тебе стоит выбрать <взять> устройство и отнести его Др. Смиту или уйти отсюда и промолчать о находке. Что ты выберешь?")
            };

            endQuestSteps[3].SetMoralChoice(new MoralChoice("взять непонятное устройство", "перейти в правый коридор", "Вы взяли устройство и вернулись на субмарину. Др. Смит решил сразу же обследовать устройство и обнаружил, что в нем заключен вирус, а оболочка нестабильна. Если бы не ваши действия вирус мог бы выбраться наружу, а теперь вы можете спокойно исследовать океан дальше.", "Вы решили не трогать устройство и вернуться на базу. Др. Смиту вы сказали, что ничего не нашли. Через некоторое время вы начали замечать мертвых существ, а после и сами по очереди замечаете у себя признаки неизвестной болезни. Вы не можете вернуться в город за помощью и теперь вам придется понять, что же вы такое проигнорировали в том храме."));

            endQuest.SetSteps(endQuestSteps);

            _quests.Add(endQuest);

            return _quests;
        }
    }
}
