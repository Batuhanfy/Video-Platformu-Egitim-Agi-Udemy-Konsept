let currentIndex = 0;

function showSlide(index) {
    const slides = document.querySelectorAll('.slide');
    if (index >= slides.length) {
        currentIndex = 0;
    } else if (index < 0) {
        currentIndex = slides.length - 1;
    } else {
        currentIndex = index;
    }
    document.querySelector('.slides').style.transform = `translateX(-${currentIndex * 100}%)`;
}

function showNextSlide() {
    showSlide(currentIndex + 1);
}

function showPrevSlide() {
    showSlide(currentIndex - 1);
}


function playVideo() {
    document.getElementById('video-player').style.display = 'block';
}
function hideVideoPlayer() {
    document.getElementById('video-player').style.display = 'none';

}

let currentIndex2= 0;

function showSlide2(index) {
    const slides2 = document.querySelectorAll('.slide2');
    if (index >= slides2.length) {
        currentIndex2 = 0;
    } else if (index < 0) {
        currentIndex2 = slides2.length - 1;
    } else {
        currentIndex2 = index;
    }
    document.querySelector('.slides2').style.transform = `translateX(-${currentIndex2 * 100}%)`;
}

function showNextSlide2() {
    showSlide2(currentIndex2 + 1);
}

function showPrevSlide2() {
    showSlide2(currentIndex2 - 1);
}