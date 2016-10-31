using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GMBuddy.Games.Dnd35;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Rest.Dnd35.Controllers
{
    [Area("Dnd35")]
    [Route("/[area]/[controller]")]
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> logger;
        private readonly Dnd35GameService games;

        public ItemsController(ILoggerFactory loggerFactory, Dnd35GameService games)
        {
            logger = loggerFactory.CreateLogger<ItemsController>();

            this.games = games;
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItem(string itemId)
        {
            var items = await games.GetItemsAsync();

            // TODO: Provide filter to games.GetItemsAsync() to do this internally
            return Json(items.Single(i => i.ItemId.ToString().Equals(itemId)));
        }

        //Should this just be called GetItems?
        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetItemsForCharacter(string characterId)
        {
            var items = await games.GetItemsAsync();
            var characterItems = items.Where(i => (i.CharacterId.ToString().Equals(characterId)));

            // TODO: Provide filter to games.GetItemsAsync() to do this internally
            return Json(characterItems);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddItem(string name, string description)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var itemId = await games.AddItemAsync(name, description);

                return CreatedAtAction(nameof(GetItem), new {Id = itemId}, null); //make this fail if item with same name exists
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        //TODO: Implement ModifyItemAttributes

        [HttpPost("")]
        public async Task<IActionResult> ModifyItemModifiers(string itemId,
        int? normalAC, int? touchAC, int? flatFootedAC, int? maxHitpoints, int? strength, int? dexterity,
        int? constitution, int? intelligence, int? wisdom, int? charisma, int? fortitudeSave, int? reflexSave,
        int? willSave, int? appraise, int? balance, int? bluff, int? climb, int? concentration, int? craft1, int? craft2,
        int? craft3, int? decipherScript, int? diplomacy, int? disableDevice, int? disguise, int? escapeArtist,
        int? forgery, int? gatherInformation, int? handleAnimal, int? heal, int? hide, int? intimidate, int? jump,
        int? knowledgeArcana, int? knowledgeArchitecture, int? knowledgeDungeoneering, int? knowledgeHistory,
        int? knowledgeLocal, int? knowledgeNature, int? knowledgeNobility, int? knowledgeThePlanes, int? knowledgeReligion,
        int? knowledgeOther, string knowledgeOtherType, int? listen, int? moveSilently, int? openLock, int? performAct,
        int? performComedy, int? performDance, int? performKeyboard, int? performOratory, int? performPercussion,
        int? performString, int? performWind, int? performSing, int? performOther, int? profession1,
        int? profession2, int? ride, int? search, int? senseMotive, int? sleightOfHand, int? spellcraft,
        int? spot, int? survival, int? swim, int? tumble, int? useMagicDevice, int? useRope)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var item = await games.ModifyItemAttributesAsync(userId, normalAC, touchAC, flatFootedAC, maxHitpoints, strength,
                dexterity, constitution, intelligence, wisdom, charisma, fortitudeSave, reflexSave, willSave, appraise, balance, bluff,
                climb, concentration, craft1, craft2, craft3, decipherScript, diplomacy, disableDevice,
                disguise, escapeArtist, forgery, gatherInformation, handleAnimal, heal, hide, intimidate, jump, knowledgeArcana, knowledgeArchitecture,
                knowledgeDungeoneering, knowledgeHistory, knowledgeLocal, knowledgeNature, knowledgeNobility, knowledgeThePlanes,
                knowledgeReligion, knowledgeOther, listen, moveSilently, openLock, performAct, performComedy,
                performDance, performKeyboard, performOratory, performPercussion, performString, performWind, performSing, performOther,
                profession1, profession2, ride, search, senseMotive, sleightOfHand, spellcraft, spot,
                survival, swim, tumble, useMagicDevice, useRope);

                return CreatedAtAction(nameof(GetItem), new {Id = item}, null);
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AssignItemToCharacter(string itemId, string characterId)
        {
            try
            {
                var item = await games.AssignItemToCharacterAsync(itemId,characterId); //not using userId because we want to allow GM to modify item list as well. How do we want to check that?
                return CreatedAtAction(nameof(GetItem), new {Id = item}, null); //Do we want to keep the concept of a CampaignCharacter and add CampaignCharacter to the database?
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }
    }
}
