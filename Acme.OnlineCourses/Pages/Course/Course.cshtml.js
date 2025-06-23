document.addEventListener('DOMContentLoaded', function () {
    const openBtn = document.getElementById('open-video-modal');
    const modal = document.getElementById('video-modal');
    const closeBtn = document.getElementById('close-video-modal');
    const iframe = document.getElementById('video-iframe');
    const youtubeUrl = "https://www.youtube.com/embed/LL35TBsClH4?autoplay=1";

    if (openBtn && modal && closeBtn && iframe) {
        openBtn.addEventListener('click', function () {
            modal.style.display = 'flex';
            iframe.src = youtubeUrl;
        });

        closeBtn.addEventListener('click', function () {
            modal.style.display = 'none';
            iframe.src = "";
        });

        // Close modal when clicking outside content
        modal.addEventListener('click', function (e) {
            if (e.target === modal) {
                modal.style.display = 'none';
                iframe.src = "";
            }
        });
    }
});