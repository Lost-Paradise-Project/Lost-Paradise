interaction-LookAt-name = Осмотреть
interaction-LookAt-description = Смотри в пустоту и смотри, как она смотрит в ответ..
interaction-LookAt-success-self-popup = Вы смотрите на { THE($target) }.
interaction-LookAt-success-target-popup = Вы чуствуете как { THE($user) } смотрит на...
interaction-LookAt-success-others-popup = { THE($user) } смотрит на { THE($target) }.
interaction-Hug-name = Обнять
interaction-Hug-description = Одно объятие в день избавляет от психологических ужасов, которые находятся за пределами вашего понимания.
interaction-Hug-success-self-popup = Вы обняли { THE($target) }.
interaction-Hug-success-target-popup = { THE($user) } Обнял вас.
interaction-Hug-success-others-popup = { THE($user) } Обнял { THE($target) }.
interaction-Pet-name = Погладить
interaction-Pet-description = Погладьте своего коллегу, чтобы облегчить его стресс.
interaction-Pet-success-self-popup = Вы гладите { THE($target) } по { POSS-ADJ($target) } голове.
interaction-Pet-success-target-popup = { THE($user) } гладит вас по { POSS-ADJ($target) } головке.
interaction-Pet-success-others-popup = { THE($user) } гладит { THE($target) }.
interaction-KnockOn-name = постучать по плечу
interaction-KnockOn-description = Постучите по плечу цели, чтобы привлечь к себе внимание.
interaction-KnockOn-success-self-popup = Вы положили руку на плечё { THE($target) }.
interaction-KnockOn-success-target-popup = { THE($user) } тронгает ваше плечё.
interaction-KnockOn-success-others-popup = { THE($user) } бъёт по плечу { THE($target) }.
interaction-Rattle-name = Грохочет
interaction-Rattle-success-self-popup = Вы грохочите на { THE($target) }.
interaction-Rattle-success-target-popup = { THE($user) } Грохочет на тебя.
interaction-Rattle-success-others-popup = { THE($user) } Грохочет на { THE($target) }.
# The below includes conditionals for if the user is holding an item
interaction-WaveAt-name = Помахать
interaction-WaveAt-description = Помашите в сторону знакомого. Если у вас в руках какой-либо предмет, он его увидят.
interaction-WaveAt-success-self-popup =
    Вы машете { $hasUsed ->
        [false] { THE($target) }.
       *[true] вашей { $used } { THE($target) }.
    }
interaction-WaveAt-success-target-popup =
    { THE($user) } машет { $hasUsed ->
        [false] at youвам.
       *[true] { POSS-PRONOUN($user) } { $used } вам.
    }
interaction-WaveAt-success-others-popup =
    { THE($user) } машет { $hasUsed ->
        [false] { THE($target) }.
       *[true] { POSS-PRONOUN($user) } { $used } { THE($target) }.
    }
