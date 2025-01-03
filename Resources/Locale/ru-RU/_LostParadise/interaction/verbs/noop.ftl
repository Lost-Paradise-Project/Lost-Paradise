interaction-LPPPatShoulder-name = Похлопать по плечу
interaction-LPPPatShoulder-description = Подбодрите кого-нибудь похлопав его по плечу!
interaction-LPPPatShoulder-success-self-popup = Вы хлопаете по плечу { THE($target) }.
interaction-LPPPatShoulder-success-target-popup = Вы чуствуете как { THE($user) } хлопает вам по плечу...
interaction-LPPPatShoulder-success-others-popup = { THE($user) } хлопает по плечу { THE($target) }.
interaction-LPPFuckYou-name = Показать средний палец
interaction-LPPFuckYou-description = Покажите свое желание послать кого-то этим действием.
interaction-LPPFuckYou-success-self-popup =
    Вы показываете средний палец { $hasUsed ->
        [false] { THE($target) }.
       *[true] держа { $used } { THE($target) }.
    }
interaction-LPPFuckYou-success-target-popup =
    { THE($user) } показывает средний палец { $hasUsed ->
        [false] вам.
       *[true] { POSS-PRONOUN($user) } { $used } вам.
    }
interaction-LPPFuckYou-success-others-popup =
    { THE($user) } показывает средний палец { $hasUsed ->
        [false] { THE($target) }.
       *[true] { POSS-PRONOUN($user) } { $used } { THE($target) }.
    }
interaction-LPPKisscheek-name = Поцеловать в щёку
interaction-LPPKisscheek-description = Наконец-то вы можете поцеловать кого-то в щёку.
interaction-LPPKisscheek-success-self-popup = Вы целуете { THE($target) } в щёку.
interaction-LPPKisscheek-success-target-popup = Вы чуствуете как { THE($user) } целует вас в щёку...
interaction-LPPKisscheek-success-others-popup = { THE($user) } целует в щёку { THE($target) }.
interaction-LPPKiss-name = Поцеловать
interaction-LPPKiss-description = Наконец-то вы можете поцеловать кого-то.
interaction-LPPKiss-success-self-popup = Вы целуете { THE($target) }.
interaction-LPPKiss-success-target-popup = Вы чуствуете как { THE($user) } целует вас...
interaction-LPPKiss-success-others-popup = { THE($user) } целует { THE($target) }.
interaction-LPPTickle-name = Щекотать
interaction-LPPTickle-description = Пощекотайте кого-то.
interaction-LPPTickle-success-self-popup = Вы щекочите { THE($target) }.
interaction-LPPTickle-success-target-popup = { THE($user) } щекочет вас.
interaction-LPPTickle-success-others-popup = { THE($user) } щекочет { THE($target) }.
interaction-LPPSlap-name = Пощёчина
interaction-LPPSlap-description = Как насчет оставить след на чужой щеке?
interaction-LPPSlap-success-self-popup = Вы наносите пощёчину { THE($target) }.
interaction-LPPSlap-success-target-popup = { THE($user) } наносит вам пощёчину.
interaction-LPPSlap-success-others-popup = { THE($user) } наносит пощёчину { THE($target) }.
interaction-LPPSlap2-name = Шлёпнуть
interaction-LPPSlap2-description = Так прекрасно, хочу шлёпнуть!
interaction-LPPSlap2-success-self-popup = Вы наносите шлепок { THE($target) }.
interaction-LPPSlap2-success-target-popup = { THE($user) } наносит вам легкий шлепок.
interaction-LPPSlap2-success-others-popup = { THE($user) } наносит легкий шлепок { THE($target) }.
interaction-LPPLick-name = Лизнуть
interaction-LPPLick-description = Фрьх~...
interaction-LPPLick-success-self-popup = Вы лизнули { THE($target) }.
interaction-LPPLick-success-target-popup = { THE($user) } лизнул вас.
interaction-LPPLick-success-others-popup = { THE($user) } лизнул { THE($target) }.
