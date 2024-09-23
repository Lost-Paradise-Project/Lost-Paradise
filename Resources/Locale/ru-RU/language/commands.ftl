command-list-langs-desc = Перечислите языки, на которых сущность может говорить.
command-list-langs-help = Исп: { $command }
command-saylang-desc = Отправьте сообщение на определенном языке. Чтобы выбрать язык, вы можете использовать либо название языка, либо его позицию в списке языков.
command-saylang-help = Исп: { $command } <имя языка> <текст>. Пример: { $command } TauCetiBasic "Привет мир!". Пример: { $command } 1 "Привет мир!"
command-language-select-desc = Выберите язык, на котором в данный момент будет говорить существо. Вы можете использовать либо название языка, либо его позицию в списке языков.
command-language-select-help = Исп: { $command } <имя языка>. Пример: { $command } TauCetiBasic
command-language-spoken = Говорит:
command-language-understood = Понимает:
command-language-current-entry = { $id }. { $language } - { $name } (текущий)
command-language-entry = { $id }. { $language } - { $name }
command-language-invalid-number = Номер языка должен быть между 0 и { $total }. Ну либо используйте имя языка.
command-language-invalid-language = Язык { $id } не существует или им нельзя говорить.

# toolshed

command-description-language-add = Добавляет новый язык к сущности. Два последних аргумента указывают, сможет ли она его произносить/понимать. Пример: 'self language:add "Canilunzt" true true'
command-description-language-rm = Удаляет язык из передаваемого объекта. Работает аналогично language:add. Пример: 'self language:rm "TauCetiBasic" true true'.
command-description-language-lsspoken = Выводит список всех языков, на которых может говорить объект. Пример: 'self language:lsspoken'
command-description-language-lsunderstood = Содержит список всех языков, которые может понимать сущность. Пример: 'self language:lssunderstood'
command-description-translator-addlang = Добавляет язык переводчику. Смотрите language:add для деталей.
command-description-translator-rmlang = Удаляет целевой язык из переводчика. Смотрите language:rm для деталей.
command-description-translator-addrequired = Добавляет новый необходимый язык к переводчику. Пример: 'ent 1234 translator:addrequired "TauCetiBasic"'
command-description-translator-rmrequired = Удаляет необходимый язык из переводчика. Пример: 'ent 1234 translator:rmrequired "TauCetiBasic"'
command-description-translator-lsspoken = Выводит список всех языков на которых говорит переводчик. Пример: 'ent 1234 translator:lsspoken'
command-description-translator-lsunderstood = Выводит список всех понятных языков для переводчика. Пример: 'ent 1234 translator:lssunderstood'
command-description-translator-lsrequired = Выводит список всех необходимых языков для переводчика. Пример: 'ent 1234 translator:lsrequired'
command-language-error-this-will-not-work = Не сработает.
command-language-error-not-a-translator = Сущность { $entity } не переводчик.
