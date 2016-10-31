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
    public class CharactersController : Controller
    {
        private readonly ILogger<CharactersController> logger;
        private readonly Dnd35GameService games;

        public CharactersController(ILoggerFactory loggerFactory, Dnd35GameService games)
        {
            logger = loggerFactory.CreateLogger<CharactersController>();

            this.games = games;
        }

        [HttpGet("")]
        public async Task<IActionResult> ListCharacters()
        {
            return Json(await games.GetCharactersAsync());
        }

        [HttpGet("{id}")] //do this in url parameters, not path
        public async Task<IActionResult> GetCharacter(string id)
        {
            var characters = await games.GetCharactersAsync();

            // TODO: Provide filter to games.GetCampaignsAsync() to do this internally
            return Json(characters.Single(c => (c.CharacterId.ToString().Equals(id))));
        }

        [HttpPost("")]
        public async Task<IActionResult> AddCharacter([FromQuery]string name)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var characterId = await games.AddCharacterAsync(userId, name);

                return CreatedAtAction(nameof(GetCharacter), new {Id = characterId}, null);
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        //TODO: Add Ability Proficiency Booleans
        [HttpPost("")]
        public async Task<IActionResult> ModifyCharacterAtributes(string characterId, string? characterClass,
        int? level, string? race, string? size, string? gender, string? allignment,
        string? diety, string? height, string? weight, string? looks, string[]? languages, string[]? feats,
        string[]? racialTraitsAndFeatures, int? spellSaveDC, int? carryCapacityLight, int? carryCapacityMedium,
        int? carryCapacityHeavy, int? experience, int? normalAC, int? touchAC, int? flatootedAC, int? maxHitpoints,
        int? currentHitpoints, int? strength, int? dexterity, int? constitution, int? intelligence, int? wisdom,
        int? charisma, int? fortitudeSave, int? reflexSave, int? willSave, int? appraise, int? balance, int? bluff,
        int? climb, int? concentration, int? craft1, string? craft1Type, int? craft2, string? craft2Type, int? craft3,
        string? craft3Type, int? decipherScript, int? diplomacy, int? disableDevice, int? disguise, int? escapeArtist,
        int? forgery, int? gatherInformation, int? handleAnimal, int? heal, int? hide, int? intimidate, int? jump,
        int? knowledgeArcana, int? knowledgeArchitecture, int? knowledgeDungeoneering, int? knowledgeHistory,
        int? knowledgeLocal, int? knowledgeNature, int? knowledgeNobility, int? knowledgeThePlanes, int? knowledgeReligion,
        int? knowledgeOther, string? knowledgeOtherType, int? listen, int? moveSilently, int? openLock, int? performAct,
        int? performComedy, int? performDance, int? performKeyboard, int? performOratory, int? performPercussion,
        int? performString, int? performWind, int? performSing, int? performOther, string? performOtherType, int? profession1,
        string? profession1Type, int? profession2, string? profession2Type, int? ride, int? search, int? senseMotive,
        int? sleightOfHand, int? spellcraft, int? spot, int? survival, int? swim, int? tumble, int? useMagicDevice,
        int? useRope)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var character = await games.ModifyAttributesAsync(userId, characterId, characterClass, level, race, size,
                gender, allignment, diety, height, weight, looks, languages, feats, racialTraitsAndFeatures, spellSaveDC, carryCapacityLight,
                carryCapacityMedium, carryCapacityHeavy, experience, normalAC, touchAC, flatootedAC, maxHitpoints, currentHitpoints, strength,
                dexterity, constitution, intelligence, wisdom, charisma, fortitudeSave, reflexSave, willSave, appraise, balance, bluff,
                climb, concentration, craft1, craft1Type, Craft2, craft2Type, craft3, craft3Type, decipherScript, diplomacy, disableDevice,
                disguise, escapeArtist, forgery, gatherInformation, handleAnimal, heal, hide, intimidate, jump, knowledgeArcana, knowledgeArchitecture,
                knowledgeDungeoneering, knowledgeHistory, knowledgeLocal, knowledgeNature, knowledgeNobility, knowledgeThePlanes,
                knowledgeReligion, knowledgeReligion, knowledgeOther, knowledgeOtherType, listen, moveSilently, openLock, performAct, performComedy,
                performDance, performKeyboard, performOratory, performPercussion, performString, performWind, performSing, performOther, performOtherType,
                profession1, profession1Type, profession2, profession2Type, ride, search, senseMotive, sleightOfHand, spellcraft, spot,
                survival, swim, tumble, useMagicDevice, useRope);

                return CreatedAtAction(nameof(GetCharacter), new {Id = character}, null);
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AssignCharacterToCampaign(string characterId, string campaignId)
        {
            try
            {
                string userId = User.Claims.Single(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                var campaign = await games.AssignCharacterToCampaignAsync(userId,characterId,campaignId);
                return CreatedAtAction(nameof(GetCampaign), new {Id = campaign}, null); //Do we want to keep the concept of a CampaignCharacter and add CampaignCharacter to the database?
            }
            catch (Exception e)
            {
                return BadRequest(new {Error = e.Message});
            }
        }
    }
}
