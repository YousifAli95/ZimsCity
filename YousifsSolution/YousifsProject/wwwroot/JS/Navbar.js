function showModal() {
    const modalBackground = document.querySelector(".modal-back");
    modalBackground.style.display = "block" 
    }

function hideModal() {
    const modalBackground = document.querySelector(".modal-back");
    modalBackground.style.display = "none"
}

modalBackground = document.querySelector('.modal-back');
if (modalBackground) {
    modalBackground.addEventListener('click', function (event) {
    var modalBack = document.querySelector('.modal-back');
    var modal = document.querySelector('.modal');

    // If the event target is not the modal, but the background, close modal
    if (event.target !== modal && event.target === modalBack) {
        hideModal();
    }
    });
}

const navBarRightSide = document.getElementById("navbar-right-side");
const navBarLeftSide = document.getElementById("navbar-left-side");

if (navBarLeftSide && navBarRightSide) {
    const width = navBarRightSide.getBoundingClientRect().width;
    widthAsRem = parseFloat(width) / 10 + "rem"
    console.log(widthAsRem)
    navBarLeftSide.style.width = widthAsRem;
}