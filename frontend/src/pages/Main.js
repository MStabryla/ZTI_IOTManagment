import './Main.css'
import React from 'react';

class Main extends React.Component {
    render(){
        return (
            <div>
                <h2>Witamy w SysOT, menadżera urządzeń IoT.</h2>
                <p>Witamy na stronie SysOT, zaawansowanego menadżera urządzeń IoT. Dzięki niemu będziesz w stanie zarządzać, zbierać i magazynować dane z wielu rodzajów czujników jak: czujniki pogodowe, czujniki dymu i gazów, mikrofony, snaery, kamery i wiele innych.</p>
                <h3>Możliwości SysOT</h3>
                <p>System SysOT umożliwia centralne zarządzanie i agregację danych z sensorów podłączonych do sieci lokalnej lub otwartej na sieć publiczną. System pozwala na rejestrację urządzeń w systemie, wraz ze szczegółowymi informacjami o ich lokalizacji i adresie. Dodatkowo system pozwala na udzielanie dostepu do zarządzania tymi urządzeniami wielu pracownikom.</p>
                <h3>Bezpieczeńśtwo transferu danych</h3>
                <p>Przesyłanie danych SysOT odbywa się poprzez autoryzację urządzenia. Każde urządzenie posiada przypisany sobie konto użytkownika, dzięki czemu system jest odporny na przyjęcie fałszywych danych od nieautoryzowanych urządzeń.</p>
            </div>
        );
    }
    
}

export default Main;