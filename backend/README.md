## SysOT - backend

Część serwerowa obejmuje stworzenie systemu API pozwalające na następujące operacje.

1. System Autoryzacji i uwierzytelniania
1. Rejestracji urządzeń IoT
    - CRUD urządzeń
    - Dopasowywanie parametrów do urządzeń
1. Zarządzania rodzajami parametrów
    - CRUD parametrów
    - Definiować sposób generowania raportów i wyników dla każdego parametru
1. Zbieranie pomiarów
    - Dodawanie pomiarów dla danego urzadzenia i w formie danego parametru
    - Zwracanie pomiarów
        - Z różnym zakresem dat
        - Z możliwością porównania między danymi z różnych urządzeń
    - Generowanie raportów
        - PDF
        - CSV, TSV