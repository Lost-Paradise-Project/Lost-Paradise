### UI

rcdfap-component-examine-mode-details = Выбран режим: '{ $mode }'.
rcdfap-component-examine-build-details = Выбран режим строительства: { $name }.

### Interaction Messages

# Mode change
rcdfap-component-change-mode = РСУАП переключён в режим '{ $mode }'.
rcdfap-component-change-build-mode = РСУАП переключён в режим строительства. Строится { $name }.
# Ammo count
rcdfap-component-no-ammo-message = В РСУАП закончились заряды!
rcdfap-component-insufficient-ammo-message = В РСУАП не хватает зарядов!
# Deconstruction
rcdfap-component-deconstruct-target-not-on-whitelist-message = Вы не можете демонтировать это!
rcdfap-component-nothing-to-deconstruct-message = Здесь нечего демонтировать!
# Construction
rcdfap-component-cannot-build-on-empty-tile-message = Это не может быть построено без фундамента.
rcdfap-component-must-build-on-subfloor-message = Это может быть построено только на покрытии!
rcdfap-component-cannot-build-on-occupied-tile-message = Здесь нельзя строить, место уже занято!

### Category names

rcd-component-DisposalPipe = Утилизационные трубы
rcd-component-Gaspipes = Газовые трубы
rcd-component-Devices = Девайсы

### Prototype names (note: constructable items will be puralized)

rcdfap-component-deconstruct = Демонтаж
rcdfap-ammo-component-on-examine =
    Содержит { $charges } { $charges ->
        [one] заряд
        [few] заряда
       *[other] зарядов
    }.
rcdfap-ammo-component-after-interact-full = РСУАП полон!
rcdfap-ammo-component-after-interact-refilled = Вы пополняете РСУАП.
