window.keyboardEvents = {
    dotNetHelper: null,
    
    initialize: function (dotNetHelper, allowedKeys) {
        window.keyboardEvents.dotNetHelper = dotNetHelper;
        
        window.addEventListener('keydown', function (e) {
            // Ignorar eventos em inputs de texto para não interferir na digitação normal (opcional, mas bom pra atalhos globais)
            // Mas para atalhos de função (F1-F12), queremos capturar sempre.
            // Para Esc, também.
            
            // Opcional: Filtrar teclas se allowedKeys for fornecido (não implementado aqui para simplicidade)

            // Previne comportamento padrão para certas teclas (F1, F5, F12 normalmente fazem coisas no navegador)
            if (e.key.startsWith('F') || e.key === 'Escape') {
                e.preventDefault();
            }

            // Mapeia para objeto simples
            let keyData = {
                key: e.key,
                code: e.code,
                keyCode: e.keyCode,
                ctrlKey: e.ctrlKey,
                shiftKey: e.shiftKey,
                altKey: e.altKey,
                metaKey: e.metaKey,
                repeat: e.repeat
            };

            // Chama método C#
            window.keyboardEvents.dotNetHelper.invokeMethodAsync('OnKeyDownJs', keyData)
                .catch(err => console.error(err));
        });
    }
};
