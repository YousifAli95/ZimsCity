document.querySelectorAll('input').forEach((input) => {
    input.addEventListener('animationstart', (event) => {
        if (event.animationName === 'autofill') {
            const label = document.getElementById(input.dataset.labelId)
            label.classList.add('autofilled');


            input.addEventListener('focusout', () => {
                const label = document.getElementById(input.dataset.labelId);
                label.classList.remove('autofilled');
            });

            input.addEventListener('input', () => {
                if (input.value === "") {
                    const label = document.getElementById(input.dataset.labelId);
                    label.classList.remove('autofilled');
                }
            });
        }
    });
});