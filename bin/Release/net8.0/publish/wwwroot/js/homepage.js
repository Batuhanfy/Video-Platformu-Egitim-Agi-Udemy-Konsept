let index = 0;

function showSlide() {
    const carousel = document.querySelector('.courses-videos');
    const totalVideos = document.querySelectorAll('.video-card').length;
    const videoWidth = document.querySelector('.video-card').offsetWidth + 20; // width + margin
    const visibleVideos = Math.floor(carousel.parentElement.offsetWidth / videoWidth);
    const maxIndex = totalVideos - visibleVideos;
    index = index < 0 ? maxIndex : index > maxIndex ? 0 : index;
    carousel.style.transform = `translateX(-${index * videoWidth}px)`;
}

function nextSlide() {
    index++;
    showSlide();
}

function prevSlide() {
    index--;
    showSlide();
}

setInterval(nextSlide, 2000); // Automatic slide every 2 seconds

window.addEventListener('resize', showSlide); // Adjust on window resize
document.addEventListener('DOMContentLoaded', showSlide); // Adjust on initial load