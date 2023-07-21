function showModal() {
    const modalBackground = document.querySelector(".modal-back");
    modalBackground.style.display = "block" 
    }

function hideModal() {
    const modalBackground = document.querySelector(".modal-back");
    modalBackground.style.display = "none"
}

document.querySelector('.modal-back').addEventListener('click', function (event) {
    var modalBack = document.querySelector('.modal-back');
    var modal = document.querySelector('.modal');

    // If the event target is not the modal, but the background, close modal
    if (event.target !== modal && event.target === modalBack) {
        hideModal();
    }
});